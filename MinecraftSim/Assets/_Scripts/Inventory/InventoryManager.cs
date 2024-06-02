using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klasa koja upravlja inventory-jem igra�a.
public class InventoryManager : MonoBehaviour
{
    // Maksimalni broj stavki koje se mogu slo�iti u jedan utor
    public int maxStackedItems = 64;
    // Polje utora inventara
    public InventorySlot[] inventorySlots;
    // Prefab stavke inventara
    public GameObject inventoryItemPrefab;

    public int selectedSlot = -1; // Trenutno odabrani utor
    public Item[] itemsToPickup;

    // Postavlja po�etno odabrani utor na prvi utor
    private void Start()
    {
        setInventoryItems();
        changeSelectedSlot(0);
    }

    // Metoda koja provjerava je li pritisnuta tipka na tipkovnici,
    // ako je onda postavlja pritisnuti slot na odabrani slot
    private void Update()
    {
        if(Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber &&  number > 0 && number < 8 )
            {
                changeSelectedSlot(number - 1);
            }
        }
    }

    public void setInventoryItems()
    {
        foreach(var item in itemsToPickup)
        {
            AddItem(item);
        }
    }
    // Metoda za promjenu odabranog utora.
    public void changeSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0) 
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        InventoryItem itemInSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();
        Debug.Log(itemInSlot.item);
    }
    // Metoda za dodavanje stavki u inventory
    public bool AddItem(Item item)
    {
        // Provjera svih utora inventory-ja
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            // Provjera je li utor prazan i odgovara li stavka koja se dodaje ve� postoje�im stavkama.
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems && itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        // Ako nije prona�en utor za postavljanje nove stavke, tra�i prazan utor i stvara novu stavku.
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot == null)
            {
                SpawnNewItem(item, slot); // Stvaranje nove stavke u prona�enom praznom utoru
                return true;
            }
        }
        return false;
    }

    // Metoda za stvaranje nove stavke u odre�enom utoru.
    void SpawnNewItem(Item item, InventorySlot slot) 
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    // Metoda za dobivanje odabrane stavke iz inventara.
    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        // Provjera postoji li stavka u odabranom utoru. Dohva�a stavku iz utora. Smanjuje broj stavki za jedan ako se stavka koristi
        // Provjera je li broj stavki u utoru pao na nulu.
        // Uni�tava stavku ako je broj stavki pao na nulu
        if (itemInSlot != null)
        {
            Item item =  itemInSlot.item;
            if(use == true)
            {
                itemInSlot.count--;
                if(itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                } else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        
        return null;
    
    }

    public BlockType GetSelectedItemType()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        // Provjera postoji li stavka u odabranom utoru. Dohva�a stavku iz utora. Smanjuje broj stavki za jedan ako se stavka koristi
        // Provjera je li broj stavki u utoru pao na nulu.
        // Uni�tava stavku ako je broj stavki pao na nulu
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            return item.type;
        }

        return BlockType.Nothing;

    }
}
