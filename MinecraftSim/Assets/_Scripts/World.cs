using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class World : MonoBehaviour
{
    // Ova klasa upravlja generacijom i upravljanjem svijeta igre

    // Veličina mape se ogledava u broju chunkova, generiraju se cijeli chunkovi
    public int mapSizeInChunks = 6;
    public int chunkSize = 16, chunkHeight = 100;

    // Definira koliko chunkova će se generirati oko igrača u svakom smjeru (gore, dolje, lijevo, desno)
    public int chunkDrawingRange = 8;

    // Prefab korišten za instanciranje chunkova
    public GameObject chunkPrefab;

    // worldRenderer varijabla služi za ponovno iskorištenje starih iskorištenih chunkova
    public WorldRenderer worldRenderer;
    public TerrainGenerator terrainGenerator;

    // Offset mape, korišten za proceduralnu generaciju
    public Vector2Int mapSeedOffset;

    // CancelationTokenSource sadrži člansku varijablu Token koji sadrži člansku varijablu IsCancellationRequested (ako je true, želi se otkazati Task)
    //Pomoću taskTokenSource će se zaustavljati task-ovi (npr. prilikom naglog zaustavljanja Unity editora)
    CancellationTokenSource taskTokenSource = new CancellationTokenSource();

    // Događaji koji se okidaju prilikom generacije svijeta i generacije novih chunkova respektivno
    public UnityEvent OnWorldCreated, OnNewChunksGenerated;

    // Struktura koja sadrži podatke o svijetu
    public WorldData worldData { get; private set; }
    public bool IsWorldCreated { get; private set; }

    private void Awake()
    {
        worldData = new WorldData
        {
            chunkHeight = this.chunkHeight,
            chunkSize = this.chunkSize,
            chunkDataDictionary = new Dictionary<Vector3Int, ChunkData>(),
            chunkDictionary = new Dictionary<Vector3Int, ChunkRenderer>()
        };
    }

    public async void GenerateWorld()
    {
        // Na početku se prosljeđuje inicijalna pozicija igrača (0)
        await GenerateWorld(Vector3Int.zero);
    }

    private async Task GenerateWorld(Vector3Int position)
    {

        terrainGenerator.GenerateBiomePoints(position, chunkDrawingRange, chunkSize, mapSeedOffset);

        // Dohvaćaju se potrebni chunkovi i podaci chunkova, prosljeđuje se i Token članska varijabla taskTokenSource-a kako bi se mogao zaustaviti Task
        WorldGenerationData worldGenerationData = await Task.Run(() => GetPositionsThatPlayerSees(position), taskTokenSource.Token);

        // Brišu se nepotrebni chunkovi
        foreach (Vector3Int pos in worldGenerationData.chunkPositionsToRemove)
        {
            WorldDataHelper.RemoveChunk(this, pos);
        }

        // Brišu se nepotrebni podaci chunkova
        foreach (Vector3Int pos in worldGenerationData.chunkDataToRemove)
        {
            WorldDataHelper.RemoveChunkData(this, pos);
        }

        /* 
            dataDictionary sadrži potrebne generirane chunkove.
            ConcurrentDictionary je threadsafe collection. Može se koristiti koliko se želi podijeliti kalkulacija potrebnih chunkova između različitih dretvi.
            U tom slučaju, te dretve mogu pristupiti dataDictionary s obzirom da je ConcurrentDictionary.
        */
        ConcurrentDictionary<Vector3Int, ChunkData> dataDictionary = null;

        try 
        {
            dataDictionary = await CalculateWorldChunkData(worldGenerationData.chunkDataPositionsToCreate);
        } 
        catch (Exception) 
        {
            Debug.Log("Task cancelled");
            return;
        }

        // Dodavanje generiranih chunkova na glavnoj dretvi (s obzirom da je dodavanje puno jeftinije od kalkulacija za generiranje chunkova)
        foreach (var calculatedData in dataDictionary)
        {
            // Key je pozicija, Value su podaci (chunkovi)
            worldData.chunkDataDictionary.Add(calculatedData.Key, calculatedData.Value);
        }

        // Nakon generacije svih potrebnih chunkova, mogu se dodati lišća. Ovo se radi kako ne bi pristupili chunku koji se još uvijek procesira na zasebnoj dretvi
        foreach (var chunkData in worldData.chunkDataDictionary.Values)
        {
            AddTreeLeafs(chunkData);
        }

        ConcurrentDictionary<Vector3Int, MeshData> meshDataDictionary = new ConcurrentDictionary<Vector3Int, MeshData>();

        // Izabiru se samo key-value parovi koji se nalaze u chunkPositionsToCreate (gleda se po poziciji), od njih se izabire vrijednost ChunkData i pretvaraju se u listu
        List<ChunkData> dataToRender = worldData.chunkDataDictionary
            .Where(keyvaluepair => worldGenerationData.chunkPositionsToCreate.Contains(keyvaluepair.Key))
            .Select(keyvaluepair => keyvaluepair.Value)
            .ToList();

        try 
        {
            meshDataDictionary = await CreateMeshDataAsync(dataToRender);
        } 
        catch (Exception) 
        {
            Debug.Log("Task cancelled");
            return;
        }

        StartCoroutine(ChunkCreationCoroutine(meshDataDictionary));
    }

    private void AddTreeLeafs(ChunkData chunkData)
    {
        // Za sve pozicije lišća, postavlja se blok lišća
        foreach (var treeLeafes in chunkData.treeData.treeLeafesSolid)
        {
            Chunk.SetBlock(chunkData, treeLeafes, BlockType.TreeLeafsSolid);
        }
    }

    private Task<ConcurrentDictionary<Vector3Int, MeshData>> CreateMeshDataAsync(List<ChunkData> dataToRender)
    {
        // Ova metoda generira potrebne mesheve za chunkove koji su potrebni. Vraća rječnik koji sadrži poziciju (ključ) i generirane mesheve (vrijednost)

        ConcurrentDictionary<Vector3Int, MeshData> dictionary = new ConcurrentDictionary<Vector3Int, MeshData>();

        return Task.Run(() => {

            // Generacija potrebnih podataka chunkova (mesheva)
            foreach (ChunkData data in dataToRender)
            {
                if (taskTokenSource.Token.IsCancellationRequested) {
                    taskTokenSource.Token.ThrowIfCancellationRequested();
                }
                MeshData meshData = Chunk.GetChunkMeshData(data);
                dictionary.TryAdd(data.worldPosition, meshData);
            }

            return dictionary;
        }, taskTokenSource.Token
        );
    }

    private Task<ConcurrentDictionary<Vector3Int, ChunkData>> CalculateWorldChunkData(List<Vector3Int> chunkDataPositionsToCreate)
    {
        // Ova metoda generira potrebne chunkove na zasebnoj dretvi, te vraća rječnik koji sadrži poziciju (ključ) i generirane chunkove (vrijednost)

        ConcurrentDictionary<Vector3Int, ChunkData> dictionary = new ConcurrentDictionary<Vector3Int, ChunkData>();

        return Task.Run(() => {
            // Prosljeđivanje taskTokenSource.Token kao drugog parametra Tasku, osigurava da će sustav automatski zaustaviti planiranje ovog zadatka (scheduling) kada se Token prekine

            foreach (Vector3Int pos in chunkDataPositionsToCreate)
            {
                if (taskTokenSource.Token.IsCancellationRequested) 
                {
                    taskTokenSource.Token.ThrowIfCancellationRequested();
                }
                ChunkData data = new ChunkData(chunkSize, chunkHeight, this, pos);
                ChunkData newData = terrainGenerator.GenerateChunkData(data, mapSeedOffset);

                // S obzirom da je ConcurrentDictionary (može se u njemu dodavati iz mnogo zasebnih dretvi), koristi se TryAdd
                dictionary.TryAdd(pos, newData);
            }
            return dictionary;
        }, taskTokenSource.Token
        );
    }

    IEnumerator ChunkCreationCoroutine(ConcurrentDictionary<Vector3Int, MeshData> meshDataDictionary)
    {
        // U ovoj metodi se za pojedini MeshData poziva CreateChunk koji renderira taj MeshData (odnosno chunk)

        foreach (var item in meshDataDictionary)
        {
            CreateChunk(worldData, item.Key, item.Value);

            // Po pojedinom frame-u se renderira 1 chunk, zaustavlja se do kraja trenutnog frame-a
            yield return new WaitForEndOfFrame();
        }

        if (IsWorldCreated == false)
        {
            IsWorldCreated = true;

            // Igrač će se spawnati tek kad se generiraju svi potrebni početni chunkovi
            OnWorldCreated?.Invoke();
        }
    }

    private void CreateChunk(WorldData worldData, Vector3Int position, MeshData meshData)
    {
        // U ovoj metodi se poziva worldRenderer.RenderChunk koji renderira pojedini MeshData. U rječnik se dodaje chunkRenderer koji je vraćen iz spomenute metode na odgovarajuću poziciju

        if (!worldData.chunkDictionary.ContainsKey(position))
        {
            ChunkRenderer chunkRenderer = worldRenderer.RenderChunk(worldData, position, meshData);
            worldData.chunkDictionary.Add(position, chunkRenderer);
        }
    }

    private WorldGenerationData GetPositionsThatPlayerSees(Vector3Int playerPosition)
    {
        /*
        Ova metoda određuje chunkove i podatke chunkova koji su potrebni. Isto tako određuje i chunkove i podatke chunkova koji su nepotrebni.
        Vraća WorldGenerationData koja sadrži sve potrebne pozicije chunkova za dodavanje i brisanje. 
        */

        // Sve pozicije koje trebaju postojati za određenu poziciju igrača (uključujući već postojeće chunkove)
        List<Vector3Int> allChunkPositionsNeeded = WorldDataHelper.GetChunkPositionsAroundPlayer(this, playerPosition);
        List<Vector3Int> allChunkDataPositionsNeeded = WorldDataHelper.GetDataPositionsAroundPlayer(this, playerPosition);

        // Sve pozicije koje se trebaju generirati za određenu poziciju igrača (uključujući već postojeće chunkove)
        List<Vector3Int> chunkPositionsToCreate = WorldDataHelper.SelectPositionsToCreate(worldData, allChunkPositionsNeeded, playerPosition);
        List<Vector3Int> chunkDataPositionsToCreate = WorldDataHelper.SelectDataPositionsToCreate(worldData, allChunkDataPositionsNeeded, playerPosition);

        List<Vector3Int> chunkPositionsToRemove = WorldDataHelper.GetUnneededChunks(worldData, allChunkPositionsNeeded);
        List<Vector3Int> chunkDataToRemove = WorldDataHelper.GetUnneededData(worldData, allChunkDataPositionsNeeded);


        WorldGenerationData data = new WorldGenerationData
        {
            chunkPositionsToCreate = chunkPositionsToCreate,
            chunkDataPositionsToCreate = chunkDataPositionsToCreate,
            chunkPositionsToRemove = chunkPositionsToRemove,
            chunkDataToRemove = chunkDataToRemove
        };
        return data;
    }

    internal BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        // Ova metoda vraća tip bloka specificiranu world koordinatama bloka 

        Vector3Int pos = Chunk.ChunkPositionFromBlockCoords(this, x, y, z);
        ChunkData containerChunk = null;

        // Pokušaj pronalaska chunkData iz rječnika koji sadrži chunkData
        worldData.chunkDataDictionary.TryGetValue(pos, out containerChunk);

        // Ukoliko chunkData ne postoji (izvan je trenutno generiranih chunkova)
        if (containerChunk == null)
            return BlockType.Nothing;

        // blockInChunkCoordinates predstavlja poziciju u lokalnom koordinatnom sustavu chunka
        Vector3Int blockInChunkCoordinates = Chunk.GetBlockInChunkCoordinates(containerChunk, new Vector3Int(x, y, z));

        // Vraća se tip bloka na toj poziciji
        return Chunk.GetBlockFromChunkCoordinates(containerChunk, blockInChunkCoordinates);
    }

    internal async void LoadAdditionalChunksRequest(GameObject player)
    {
        // Ova metoda se okida za stvaranje chunkova oko igrača

        Debug.Log("Load more chunks");
        await GenerateWorld(Vector3Int.RoundToInt(player.transform.position));
        OnNewChunksGenerated?.Invoke();
    }
    internal bool SetBlock(RaycastHit hit, BlockType blockType, bool isBuilding)
    {
        ChunkRenderer chunk = hit.collider.GetComponent<ChunkRenderer>();
        if (chunk == null)
            return false;
        Vector3Int pos;

        if (isBuilding)
        {
            pos = GetAdjacentBlockPos(hit);
        }
        else
        {
            pos = GetBlockPos(hit);
        }
        

        WorldDataHelper.SetBlock(chunk.ChunkData.worldReference, pos, blockType);
        chunk.modifiedByThePlayer = true;

        if(Chunk.IsOnTheEdge(chunk.ChunkData, pos))
        {
            List<ChunkData> neighbourDataList = Chunk.GetEdgeNeighbourChunk(chunk.ChunkData, pos);
            foreach(var neighbourData in neighbourDataList)
            {
                //neighbourData.modifiedByThePlayer = true;
                ChunkRenderer chunkToUpdate = WorldDataHelper.GetChunk(neighbourData.worldReference, neighbourData.worldPosition);
                if (chunkToUpdate != null)
                    chunkToUpdate.UpdateChunk();
            }
        }

        chunk.UpdateChunk();
        return true;
    }

    private Vector3Int GetAdjacentBlockPos(RaycastHit hit)
    {
        Vector3 hitPos = hit.point;
        Vector3 normal = hit.normal;
        Vector3 adjacentBlockPos = hitPos + normal * 0.5f;

        return Vector3Int.RoundToInt(adjacentBlockPos);
    }

    private Vector3Int GetBlockPos(RaycastHit hit)
    {
        Vector3 pos = new Vector3(
            GetBlockPositionIn(hit.point.x, hit.normal.x),
            GetBlockPositionIn(hit.point.y, hit.normal.y),
            GetBlockPositionIn(hit.point.z, hit.normal.z)
        );

        return Vector3Int.RoundToInt(pos);
    }

    private float GetBlockPositionIn(float pos, float normal)
    {
        if (Mathf.Abs(pos % 1) == 0.5f)
        {
            pos -= (normal / 2);
        }

        return (float)pos;
    }

    public void OnDisable()
    {
        // Ova metoda se poziva kada se zaustavi editor (zajedno sa metodom OnDestroy())

        // isCancellationRequested unutar Token članske varijable se postavlja na true
        taskTokenSource.Cancel();
    }

    public struct WorldGenerationData
    {
        public List<Vector3Int> chunkPositionsToCreate;
        public List<Vector3Int> chunkDataPositionsToCreate;
        public List<Vector3Int> chunkPositionsToRemove;
        public List<Vector3Int> chunkDataToRemove;
    }
}

public struct WorldData
{
    public Dictionary<Vector3Int, ChunkData> chunkDataDictionary;
    public Dictionary<Vector3Int, ChunkRenderer> chunkDictionary;
    public int chunkSize;
    public int chunkHeight;
}
