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
    // Boje za ozna�avanje odabranog i neodabranog stanja
    public Color selectedColor, notSelectedColor;

    // Metoda koja poziva metodu za odzna�avanje utora
    private void Awake()
    {
        Deselect();
    }
    // Metoda za ozna�avanje utora.
    public void Select()
    {
        image.color = selectedColor;
    }
    // Metoda za poni�tavanje ozna�avanja utora.
    public void Deselect()
    {
        image.color = notSelectedColor;
    }
    // Metoda koja se poziva kada se stavka ispusti u utor.
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            // Dohva�a komponentu InventoryItem iz stavke koja se povla�i.
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }

}
