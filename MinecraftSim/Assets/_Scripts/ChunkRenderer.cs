using UnityEngine;
using System.Linq;
using UnityEditor;

// Pomoću RequireComponent, kad god se dodaje ChunkRenderer nekom GameObject-u u Unityju, automatski se dodaju komponente specificirane njime  
[RequireComponent(typeof(MeshFilter))] // MeshFilter-u se predaje kreirani mesh, predaje taj mesh MeshRenderer-u za renderiranje na zaslonu
[RequireComponent(typeof(MeshRenderer))] // Uzima geometriju iz MeshFilter-a te renderira mesh u poziciji definiranoj GameObject Transfrom komponentom
[RequireComponent(typeof(MeshCollider))] // Služi za generiranje kolizija između tla i igrača
public class ChunkRenderer : MonoBehaviour
{
    // ChunkRenderer enkapsulira logiku za renderiranje chunkova.

    MeshFilter meshFilter;
    MeshCollider meshCollider;
    Mesh mesh;

    // Omogućuje prikazivanje veličine cijelog chunka
    public bool showGizmo = false;

    // Na temelju ChunkData, generira se MeshData
    public ChunkData ChunkData {get; private set;}

    public bool modifiedByThePlayer {
        get {
            return ChunkData.modifiedByThePlayer;
        }
        set {
            ChunkData.modifiedByThePlayer = value;
        }
    }

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;
    }

    public void InitializeChunk(ChunkData data) {
        // Konstruktor

        this.ChunkData = data;
    }

    private void RenderMesh(MeshData meshData) {

        // Briše se trenutni mesh
        mesh.Clear();

        // Definiraju se 2 submesheva (1 mesh objekt može imati nekoliko mesheva, svaki od mesheva se povezuje s određenim materijalom)
        mesh.subMeshCount = 2;

        // U istom polju se postavljaju općeniti vrhovi i vrhovi iz waterMesha
        mesh.vertices = meshData.vertices.Concat(meshData.waterMesh.vertices).ToArray();

        // Svaki submesh ima svoje trokute postavljene zasebno
        mesh.SetTriangles(meshData.triangles.ToArray(), 0);

        // Svakom indeksu unutar waterMesh.triangles se prirodaje offset, odgovarat će indeksima vrhova unutar mesh.vertices
        mesh.SetTriangles(meshData.waterMesh.triangles.Select(val => val + meshData.vertices.Count).ToArray(), 1);

        mesh.uv = meshData.uv.Concat(meshData.waterMesh.uv).ToArray();

        // Za ispravnu refleksiju svijetla
        mesh.RecalculateNormals();

        // meshCollider.sharedMesh je objekt korišten za detekciju kolizija
        meshCollider.sharedMesh = null;
        Mesh collisionMesh = new Mesh();
        collisionMesh.vertices = meshData.colliderVertices.ToArray();
        collisionMesh.triangles = meshData.colliderTriangles.ToArray();
        collisionMesh.RecalculateNormals();

        meshCollider.sharedMesh = collisionMesh;
    }

    public void UpdateChunk() {
        // Metoda koja se koristi ukoliko se želi modificirati chunk, te se izračuna MeshData na istoj dretvi

        RenderMesh(Chunk.GetChunkMeshData(ChunkData));
    }

    public void UpdateChunk(MeshData data) {
        // Metoda koja prima MeshData izračunate na zasebnoj dretvi
        
        RenderMesh(data);
    }

    // Omogućava izvršavanje samo u Unity editoru, s obzirom da se Selection nalazi u Unityju 
    #if UNITY_EDITOR

    private void OnDrawGizmos()
    // Metoda koja omogućava vizualizaciju chunka, može se usporediti koliko je chunk velik s obzirom na ono što je renderirano
    {
        if (showGizmo)
        {
            if (Application.isPlaying && ChunkData != null)
            {
                // Ukoliko je izabran chunk, razlikovat će se od ostalih chunkova
                if (Selection.activeObject == gameObject)
                    Gizmos.color = new Color(0, 1, 0, 0.4f); // zelena boja
                else 
                    Gizmos.color = new Color(1, 0, 1, 0.4f); // magenta boja
                
                // Prvi parametar je centar chunka, drugi parametar specificira veličinu kocke
                Gizmos.DrawCube(transform.position + new Vector3(ChunkData.chunkSize / 2f, ChunkData.chunkHeight / 2f, ChunkData.chunkSize / 2f), 
                new Vector3(ChunkData.chunkSize, ChunkData.chunkHeight, ChunkData.chunkSize));
            }
        }
    }
    #endif
}
