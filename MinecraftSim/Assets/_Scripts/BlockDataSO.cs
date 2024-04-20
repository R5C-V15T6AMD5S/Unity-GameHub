using System;
using System.Collections.Generic;
using UnityEngine;

// Atribut koji nam omogućava kreiranje Block Data kroz Asset Menu
[CreateAssetMenu(fileName ="Block Data" ,menuName ="Data/Block Data")]
public class BlockDataSO : ScriptableObject
{
    // BlockDataSO sadrži podatke o voxelima, odnosno tipovima blokova

    // Ove 2 varijable definiraju korak pri biranju tekstura
    public float textureSizeX, textureSizeY;

    // Ovo je lista tipova blokova nad kojim su definirane teksture na vrhu, dnu i stranama
    public List<TextureData> textureDataList;
}

[Serializable]
public class TextureData
{
    public BlockType blockType;

    // UV koordinate za specifičnu teksturu, voxeli imaju iste teksture na svojim bočnim stranama
    public Vector2Int up, down, side;

    // isSolid vode je false, a tla je true
    public bool isSolid = true;

    // Odgovara na pitanje koji voxel će generirati collider
    public bool generatesCollider = true;
}

