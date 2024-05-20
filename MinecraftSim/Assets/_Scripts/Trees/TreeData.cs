using System.Collections.Generic;
using UnityEngine;

public class TreeData
{
    // treePositions su neovisni o y komponenti (visini), generiraju se odabirom slučajne pozicije pomoću Perlinovog šuma.
    public List<Vector2Int> treePositions = new List<Vector2Int>();
    // treeLeafesSolid je indikator gdje postaviti voxel koji predstavlja lišće drveća
    public List<Vector3Int> treeLeafesSolid = new List<Vector3Int>();
}
