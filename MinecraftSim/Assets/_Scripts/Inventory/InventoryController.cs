using UnityEngine;

// Metoda koja slu�i za kontroliranje i prikazivanje inventory-ja
public class InventoryController : MonoBehaviour
{
    // Referenca na UI element inventory-ja
    public GameObject inventoryUI;

    // Varijabla za pra�enje stanja prikaza inventory-ja
    private bool isInventoryVisible = false;

    void Start()
    {
        // Po�etno stanje: inventory je skriven
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        // Prikazivanje/skrivanje inventory-ja na pritisak tipke E
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }

        // Ako je inventory aktivan, postavi kursor vidljivim i omogu�i slobodno kretanje kursora
        if (isInventoryVisible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else // Ina�e, sakrij kursor i zaklju�aj ga u sredi�te ekrana
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Metoda za prikazivanje/skrivanje inventory
    void ToggleInventory()
    {
        // Ako je inventory aktivan, skrij ga ako nije, prika�i ga
        inventoryUI.SetActive(!inventoryUI.activeSelf);

        // A�uriraj stanje prikaza inventory
        isInventoryVisible = inventoryUI.activeSelf;
    }
}
