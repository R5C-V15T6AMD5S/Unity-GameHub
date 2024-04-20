using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // TerrainGenerator upravlja stvaranjem terenskih podataka za chunk
    public BiomeGenerator biomeGenerator;
    public ChunkData GenerateChunkData(ChunkData data, Vector2Int mapSeedOffset)
    {
        for (int x = 0; x < data.chunkSize; x++)
        {
            for (int z = 0; z < data.chunkSize; z++)
            {
                // Za svaku poziciju potrebno je terenske podatke
                data = biomeGenerator.ProcessChunkColumn(data, x, z, mapSeedOffset);
            }
        }
        return data;
    }
}
