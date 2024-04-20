using UnityEngine;

public abstract class BlockLayerHandler : MonoBehaviour
{
    // BlockLayerHandler definira osnovno ponašanje za Handler-e

    // Pokazivač na sljedeći Handler u hijerarhiji
    [SerializeField]
    private BlockLayerHandler Next;
    public bool Handle(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        // Ova metoda provjerava je li trenutni Handler ispravan za određenu visinu. Ukoliko nije prelazi se na sljedećeg Handlera u nizu.

        if (TryHandling(chunkData, x, y, z, surfaceHeightNoise, mapSeedOffset)) return true;
        if (Next != null) return Next.Handle(chunkData, x, y, z, surfaceHeightNoise, mapSeedOffset);
        return false;
    }

    // Metoda koja će probati postaviti tip bloka ukoliko y odgovara definiranom rasponu unutar Handlera.
    protected abstract bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset);
}
