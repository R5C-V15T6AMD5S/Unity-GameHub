using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Klasa za prikazivanje i ponašanje predmeta inventory-ja
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
    // Definiranje transformacije roditelja nakon povlaèenja
    [HideInInspector] public Transform parentAfterDrag;
    // Inicijalizacije predmeta inventory-ja i postavljanje slike predmeta
    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;  
        RefreshCount();
    }
    // Osvježavanje teksta brojaèa
    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }
    // Metoda koja se poziva pri povlaèenju predmeta
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Onemoguæavanje ciljanje zrake na slici kako bi se omoguæila interakcija s stavkama iza nje
        image.raycastTarget = false; 
        // Sprema trenutnu transformaciju roditelja prije povlaèenja
        parentAfterDrag = transform.parent;
        // Postavlja roditeljsku transformaciju na korijensku transformaciju kako bi bila prikazana na vrhu
        transform.SetParent(transform.root);
    }

    // Poziva se tijekom povlaèenja stavke inventara
    // Pomièe stavku na trenutnu poziciju miša
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    // Poziva se kada se povlaèenje završi na stavci inventara
    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        // Postavlja transformaciju roditelja natrag na izvornog roditelja nakon povlaèenja
        transform.SetParent(parentAfterDrag);
    }
}
