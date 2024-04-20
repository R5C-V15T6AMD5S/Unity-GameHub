using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Klasa za prikazivanje i pona�anje predmeta inventory-ja
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    // Referenca na prikaz slike predmeta
    public Image image;
    // Referenca na prikaz broja vrsta istog predmeta koje imamo u inventory-ju
    public Text countText;

    // Definiranje predmeta
    [HideInInspector] public Item item;
    // Definniranje broja 
    [HideInInspector] public int count = 1;
    // Definiranje transformacije roditelja nakon povla�enja
    [HideInInspector] public Transform parentAfterDrag;
    // Inicijalizacije predmeta inventory-ja i postavljanje slike predmeta
    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;  
        RefreshCount();
    }
    // Osvje�avanje teksta broja�a
    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }
    // Metoda koja se poziva pri povla�enju predmeta
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Onemogu�avanje ciljanje zrake na slici kako bi se omogu�ila interakcija s stavkama iza nje
        image.raycastTarget = false; 
        // Sprema trenutnu transformaciju roditelja prije povla�enja
        parentAfterDrag = transform.parent;
        // Postavlja roditeljsku transformaciju na korijensku transformaciju kako bi bila prikazana na vrhu
        transform.SetParent(transform.root);
    }

    // Poziva se tijekom povla�enja stavke inventara
    // Pomi�e stavku na trenutnu poziciju mi�a
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    // Poziva se kada se povla�enje zavr�i na stavci inventara
    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        // Postavlja transformaciju roditelja natrag na izvornog roditelja nakon povla�enja
        transform.SetParent(parentAfterDrag);
    }
}
