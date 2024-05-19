using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLayerHandler : BlockLayerHandler
{
    // Klasa TreeLayerHandler provjerava može li se na određenoj poziciji u chunku postaviti stablo, te ukoliko može, postavlja ga

    // Visina iznad koje se ne postavljaju drveća
    public float terrainHeightLimit = 25;

    // Lista pozicija lišća drveća
    public static List<Vector3Int> treeLeafesStaticLayout = new List<Vector3Int>
    {
        new Vector3Int(-2, 0, -2),
        new Vector3Int(-2, 0, -1),
        new Vector3Int(-2, 0, 0),
        new Vector3Int(-2, 0, 1),
        new Vector3Int(-2, 0, 2),
        new Vector3Int(-1, 0, -2),
        new Vector3Int(-1, 0, -1),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(-1, 0, 2),
        new Vector3Int(0, 0, -2),
        new Vector3Int(0, 0, -1),
        new Vector3Int(0, 0, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, 2),
        new Vector3Int(1, 0, -2),
        new Vector3Int(1, 0, -1),
        new Vector3Int(1, 0, 0),
        new Vector3Int(1, 0, 1),
        new Vector3Int(1, 0, 2),
        new Vector3Int(2, 0, -2),
        new Vector3Int(2, 0, -1),
        new Vector3Int(2, 0, 0),
        new Vector3Int(2, 0, 1),
        new Vector3Int(2, 0, 2),
        new Vector3Int(-1, 1, -1),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(-1, 1, 1),
        new Vector3Int(0, 1, -1),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, 1, 1),
        new Vector3Int(1, 1, -1),
        new Vector3Int(1, 1, 0),
        new Vector3Int(1, 1, 1),
        new Vector3Int(0, 2, 0)
    };

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        // Ispod površine se neće postavljati drveća
        if (chunkData.worldPosition.y < 0) return false;

        if (surfaceHeightNoise < terrainHeightLimit && chunkData.treeData.treePositions.Contains(new Vector2Int(chunkData.worldPosition.x + x, chunkData.worldPosition.z + z)))
        {
            // Provjera koji je tip bloka ispod stabla (postavlja se stablo samo ako je Grass_Dirt)
            Vector3Int chunkCoordinates = new Vector3Int(x, surfaceHeightNoise, z);
            BlockType type = Chunk.GetBlockFromChunkCoordinates(chunkData, chunkCoordinates);

            if (type == BlockType.Grass_Dirt)
            {
                // Ispod tla postavljamo Dirt
                Chunk.SetBlock(chunkData, chunkCoordinates, BlockType.Dirt);
                for (int i = 1; i < 5; i++)
                {
                    // Postavlja se stablo
                    chunkCoordinates.y = surfaceHeightNoise + i;
                    Chunk.SetBlock(chunkData, chunkCoordinates, BlockType.TreeTrunk);
                }

                foreach (Vector3Int leafPosition in treeLeafesStaticLayout)
                {
                    // Postavljaju se lišća za pojedino stablo
                    chunkData.treeData.treeLeafesSolid.Add(new Vector3Int(x + leafPosition.x, surfaceHeightNoise + 5 + leafPosition.y, z + leafPosition.z));
                }
            }
        }
        return false;
    }
}
