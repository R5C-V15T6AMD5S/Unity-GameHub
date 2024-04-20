using System.Collections.Generic;
using UnityEngine;

public class BlockDataManager : MonoBehaviour
{
    // BlockDataManager centralizira upravljanje teksturama blokova tako što ih sprema u rječnik za lagan pristup.

    // textureOffset varijabla pomaže pri prikazu tekstura, rješava bug pri kojem teksture prikazuju i teksture pored sebe (texture bleeding ili artifacts)
    public static float textureOffset = 0.001f;

    // Sadrže vrijednosti veličine pločica za teksture
    public static float tileSizeX, tileSizeY;

    // Omogućava lagan pristup TextureData baziranim na tipovima blokova
    public static Dictionary<BlockType, TextureData> blockTextureDataDictionary = new Dictionary<BlockType, TextureData>();
    public BlockDataSO textureData;

    private void Awake()
    {
        // Za svaki tip bloka, provjerava nalazi li se u rječniku. Ako nije, dodaje tip bloka kao ključ, a sam pripadajući TextureData kao vrijednost

        foreach (var item in textureData.textureDataList)
        {
            if (blockTextureDataDictionary.ContainsKey(item.blockType) == false)
            {
                blockTextureDataDictionary.Add(item.blockType, item);
            };
        }
        tileSizeX = textureData.textureSizeX;
        tileSizeY = textureData.textureSizeY;
    }
}
