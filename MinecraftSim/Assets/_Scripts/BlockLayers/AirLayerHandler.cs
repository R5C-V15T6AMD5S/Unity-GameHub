using UnityEngine;

public class AirLayerHandler : BlockLayerHandler
{
    // Ovaj Handler postavlja tip bloka kao "zrak" ukoliko je y pozicija iznad same površine.
    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (y > surfaceHeightNoise)
        {
            Vector3Int pos = new Vector3Int(x, y, z);
            Chunk.SetBlock(chunkData, pos, BlockType.Air);
            return true;
        }
        return false;
    }
}
