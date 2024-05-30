using UnityEngine;

public class UndergroundLayerHandler : BlockLayerHandler
{
    // Ovaj Handler postavlja podzemni tip bloka na onaj koji je korisnik definirao unutar Unityja kao "Underground Block Type"
    public BlockType undergroundBlockType;
    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (y < surfaceHeightNoise)
        {
            Vector3Int pos = new Vector3Int(x, y - chunkData.worldPosition.y, z);
            Chunk.SetBlock(chunkData, pos, undergroundBlockType);
            return true;
        }
        return false;
    }
}
