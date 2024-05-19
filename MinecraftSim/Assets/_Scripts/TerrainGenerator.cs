using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // TerrainGenerator upravlja stvaranjem terenskih podataka za chunk
    public BiomeGenerator biomeGenerator;
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
}
