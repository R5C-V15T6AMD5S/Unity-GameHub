// InventoryManagerTests.cs
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class InventoryManagerTests
{
    private GameObject inventoryManagerObject;
    private InventoryManager inventoryManager;

    [SetUp]
    public void Setup()
    {
        // Kreirajte GameObject za InventoryManager
        inventoryManagerObject = new GameObject();
        inventoryManager = inventoryManagerObject.AddComponent<InventoryManager>();

        // Kreirajte slotove inventara
        inventoryManager.inventorySlots = new InventorySlot[5];
        for (int i = 0; i < inventoryManager.inventorySlots.Length; i++)
        {
            GameObject slotObject = new GameObject();
            InventorySlot slot = slotObject.AddComponent<InventorySlot>();

            // Postavite Image komponentu
            slot.image = slotObject.AddComponent<Image>();
            slot.selectedColor = Color.red;
            slot.notSelectedColor = Color.white;

            inventoryManager.inventorySlots[i] = slot;
        }

        // Kreirajte prefab za InventoryItem
        GameObject inventoryItemPrefab = new GameObject();
        InventoryItem inventoryItem = inventoryItemPrefab.AddComponent<InventoryItem>();

        // Postavite countText komponentu
        GameObject textObject = new GameObject();
        Text countText = textObject.AddComponent<Text>();
        inventoryItem.countText = countText;

        // Postavite itemImage komponentu
        GameObject imageObject = new GameObject();
        Image itemImage = imageObject.AddComponent<Image>();
        inventoryItem.image = itemImage;

        // Postavite inventoryItemPrefab u InventoryManager
        inventoryManager.inventoryItemPrefab = inventoryItemPrefab;

        // Kreirajte stavke za pickup
        Item Stone = ScriptableObject.CreateInstance<Item>();
        Stone.name = "Stone";
        Stone.stackable = true;
        Stone.type = BlockType.Stone;
        Stone.image = null; // Postavite odgovarajuæi sprite

        Item Sand = ScriptableObject.CreateInstance<Item>();
        Sand.name = "Sand";
        Sand.stackable = true;
        Sand.type = BlockType.Sand;
        Sand.image = null; // Postavite odgovarajuæi sprite

        inventoryManager.itemsToPickup = new Item[] { Stone, Sand };
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(inventoryManagerObject);
    }

    [UnityTest]
    public IEnumerator TestInventoryItems()
    {
        inventoryManager.setInventoryItems();
        yield return null;

        Assert.NotNull(inventoryManager.inventorySlots[0].GetComponentInChildren<InventoryItem>());
        Assert.AreEqual("Stone", inventoryManager.inventorySlots[0].GetComponentInChildren<InventoryItem>().item.name);
    }

    [UnityTest]
    public IEnumerator TestChangeSelectedSlot()
    {
        inventoryManager.changeSelectedSlot(1);
        yield return null;

        Assert.AreEqual(BlockType.Sand, inventoryManager.GetSelectedItemType());
    }

    [UnityTest]
    public IEnumerator TestAddItem()
    {
        Item newItem = ScriptableObject.CreateInstance<Item>();
        newItem.name = "NewItem";
        newItem.stackable = true;
        newItem.type = BlockType.Nothing;
        newItem.image = null; // Postavite odgovarajuæi sprite

        bool result = inventoryManager.AddItem(newItem);
        yield return null;

        Assert.IsTrue(result);
    }

    [UnityTest]
    public IEnumerator TestGetSelectedItem()
    {
        inventoryManager.changeSelectedSlot(0);
        inventoryManager.setInventoryItems();
        yield return null;

        Item selectedItem = inventoryManager.GetSelectedItem(false);
        Assert.NotNull(selectedItem);
        Assert.AreEqual("Stone", selectedItem.name);
    }
}
