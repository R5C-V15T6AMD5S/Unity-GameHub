using System;
using System.Collections;
using System.Collections.Generic;
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
    public TerrainGenerator terrainGenerator;

    // Offset mape, korišten za proceduralnu generaciju
    public Vector2Int mapSeedOffset;

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

    public void GenerateWorld()
    {
        // Na početku se prosljeđuje inicijalna pozicija igrača (0)
        GenerateWorld(Vector3Int.zero);
    }

    private void GenerateWorld(Vector3Int position)
    {
        // Dohvaćaju se potrebni chunkovi i podaci chunkova
        WorldGenerationData worldGenerationData = GetPositionsThatPlayerSees(position);

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

        // Generacija potrebnih chunkova
        foreach (var pos in worldGenerationData.chunkDataPositionsToCreate)
        {
            ChunkData data = new ChunkData(chunkSize, chunkHeight, this, pos);
            ChunkData newData = terrainGenerator.GenerateChunkData(data, mapSeedOffset);
            worldData.chunkDataDictionary.Add(pos, newData);
        }

        Dictionary<Vector3Int, MeshData> meshDataDictionary = new Dictionary<Vector3Int, MeshData>();

        // Generacija potrebnih podataka chunkova
        foreach (Vector3Int pos in worldGenerationData.chunkPositionsToCreate)
        {
            ChunkData data = worldData.chunkDataDictionary[pos];
            MeshData meshData = Chunk.GetChunkMeshData(data);
            meshDataDictionary.Add(pos, meshData);
        }

        StartCoroutine(ChunkCreationCoroutine(meshDataDictionary));
    }

    IEnumerator ChunkCreationCoroutine(Dictionary<Vector3Int, MeshData> meshDataDictionary)
    {
        foreach (var item in meshDataDictionary)
        {
            CreateChunk(worldData, item.Key, item.Value);

            // Po pojedinom frame-u se renderira 1 chunk
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
        GameObject chunkObject = Instantiate(chunkPrefab, position, Quaternion.identity);
        ChunkRenderer chunkRenderer = chunkObject.GetComponent<ChunkRenderer>();
        worldData.chunkDictionary.Add(position, chunkRenderer);
        chunkRenderer.InitializeChunk(worldData.chunkDataDictionary[position]);
        chunkRenderer.UpdateChunk(meshData);
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

    internal void LoadAdditionalChunksRequest(GameObject player)
    {
        // Ova metoda se okida za stvaranje chunkova oko igrača

        Debug.Log("Load more chunks");
        GenerateWorld(Vector3Int.RoundToInt(player.transform.position));
        OnNewChunksGenerated?.Invoke();
    }

    internal void RemoveChunk(ChunkRenderer chunk)
    {
        chunk.gameObject.SetActive(false);
    }

    public struct WorldGenerationData
    {
        public List<Vector3Int> chunkPositionsToCreate;
        public List<Vector3Int> chunkDataPositionsToCreate;
        public List<Vector3Int> chunkPositionsToRemove;
        public List<Vector3Int> chunkDataToRemove;
    }

    public struct WorldData
    {
        public Dictionary<Vector3Int, ChunkData> chunkDataDictionary;
        public Dictionary<Vector3Int, ChunkRenderer> chunkDictionary;
        public int chunkSize;
        public int chunkHeight;
    }
}
