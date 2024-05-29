using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // TerrainGenerator upravlja stvaranjem terenskih podataka za chunk
    public BiomeGenerator biomeGenerator;

    [SerializeField]
    List<Vector3Int> biomeCenters = new List<Vector3Int>();
    public ChunkData GenerateChunkData(ChunkData data, Vector2Int mapSeedOffset)
    {
        // Linije koda koje se tiču treeData su postavljene prije 2 for petlje iz razloga jer se želi jednom izračunati vrijednosti šuma.

        // Generiraju se podaci o stablima, razliku može činiti sam mapSeedOffset.
        TreeData treeData = biomeGenerator.GetTreeData(data, mapSeedOffset);
        // Podaci o stablima se spremaju u ChunkData. Razlog tome je da prilikom renderiranja chunkova, treba uključiti samo lišće koje je vidljivo u odgovarajućem chunku.
        data.treeData = treeData;

        for (int x = 0; x < data.chunkSize; x++)
        {
            for (int z = 0; z < data.chunkSize; z++)
            {
                // Za svaku poziciju potrebno je generirati terenske podatke
                data = biomeGenerator.ProcessChunkColumn(data, x, z, mapSeedOffset);
            }
        }
        return data;
    }

    public void GenerateBiomePoints(Vector3 playerPosition, int drawRange, int mapSize, Vector2Int mapSeedOffset)
    {

        // Resetiraju se pozicije
        biomeCenters = new List<Vector3Int>();

        biomeCenters = BiomeCenterFinder.CalculateBiomeCenters(playerPosition, drawRange, mapSize);
    }

    private void OnDrawGizmos()
    {
        // funkcija kojom se vizualiziraju središnje pozicije bioma

        Gizmos.color = Color.blue;

        foreach (var biomCenterPoint in biomeCenters)
        {
            Gizmos.DrawLine(biomCenterPoint, biomCenterPoint + Vector3.up * 255);
        }
    }
}
