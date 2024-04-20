using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Klasa za definiranje predmeta inventory-ja
[CreateAssetMenu(menuName = "Scriptable object/item")]
public class Item: ScriptableObject
{
    // Definiranje vrste i osnovnih postavki predmeta
    [Header("Only gameplay")]
    public TileBase tile;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    
    // Definiranje može li više predmeta stack-at
    [Header("Only UI")]
    public bool stackable = true;

    // Postavljanje slike predmeta
    [Header("Both")]
    public Sprite image;
}

// Enumeracija tipa predmeta
public enum ItemType
{
    BuildingBlock,
    Tool
}

// Enumeracija akcije predmeta
public enum ActionType
{
    Dig,
    Mine
}
