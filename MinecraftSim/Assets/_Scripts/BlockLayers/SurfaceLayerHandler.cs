using UnityEngine;

public class SurfaceLayerHandler : BlockLayerHandler
{
    // Ovaj Handler postavlja površinski tip bloka na onaj koji je korisnik definirao unutar Unityja kao "Surface Block Type"
    public BlockType surfaceBlockType;
    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if(y == surfaceHeightNoise)
        {
            Vector3Int pos = new Vector3Int(x, y, z);
            Chunk.SetBlock(chunkData, pos, surfaceBlockType);
            return true;
        }
        return false;
    }
}
