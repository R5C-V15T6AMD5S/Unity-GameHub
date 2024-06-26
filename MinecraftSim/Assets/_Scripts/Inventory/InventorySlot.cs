using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Klasa koja predstavlja utor koji je trenutno odabran inventory-ja 
public class InventorySlot : MonoBehaviour, IDropHandler
{
    // Referenca na komponentu slike za prikaz utora
    public Image image;
    // Boje za označavanje odabranog i neodabranog stanja
    public Color selectedColor, notSelectedColor;

    // Metoda koja poziva metodu za odznačavanje utora
    private void Awake()
    {
        Deselect();
    }
    // Metoda za označavanje utora.
    public void Select()
    {
        image.color = selectedColor;
    }
    // Metoda za poništavanje označavanja utora.
    public void Deselect()
    {
        image.color = notSelectedColor;
    }
    // Metoda koja se poziva kada se stavka ispusti u utor.
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            // Dohvaća komponentu InventoryItem iz stavke koja se povlači.
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }

}
