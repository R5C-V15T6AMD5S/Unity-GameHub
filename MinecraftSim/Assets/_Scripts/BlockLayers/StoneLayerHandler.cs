using UnityEngine;

public class StoneLayerHandler : BlockLayerHandler
{
    // U ovom Handler-u vršimo izmjenu postojećih blokova sa "kamenim" blokom

    // Threshold pomoću kojeg se odlučuje treba li postaviti kameni blok
    [Range(0, 1)]
    public float stoneThreshold = 0.5f;

    [SerializeField]
    private NoiseSettings stoneNoiseSettings;

    public DomainWarping domainWarping;
    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {

        // Neće se zamijeniti "zrak" u "kamen"
        if (chunkData.worldPosition.y > surfaceHeightNoise) return false;

        stoneNoiseSettings.worldOffset = mapSeedOffset;

        float stoneNoise = domainWarping.GenerateDomainNoise(chunkData.worldPosition.x + x, chunkData.worldPosition.z + z, stoneNoiseSettings);

        // Pozicija do koje se mogu postaviti kameni blokovi.
        int endPosition = surfaceHeightNoise;

        if (chunkData.worldPosition.y < 0)
        {
            // Svi blokovi ispod terena se postavljaju kao "kameni"
            endPosition = chunkData.worldPosition.y + chunkData.chunkHeight;
        }

        if (stoneNoise > stoneThreshold)
        {
            // Postavljanje blokova uzduž cijelgo stupca chunka kao "kameni" tip
            for (int i = chunkData.worldPosition.y; i <= endPosition; i++)
            {
                Vector3Int pos = new Vector3Int(x, i, z);
                Chunk.SetBlock(chunkData, pos, BlockType.Stone);
            }
            return true;
        }
        return false;
    }
}
