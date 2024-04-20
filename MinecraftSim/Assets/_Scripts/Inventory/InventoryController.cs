using UnityEngine;

// Metoda koja služi za kontroliranje i prikazivanje inventory-ja
public class InventoryController : MonoBehaviour
{
    // Referenca na UI element inventory-ja
    public GameObject inventoryUI;

    // Varijabla za praæenje stanja prikaza inventory-ja
    private bool isInventoryVisible = false;

    void Start()
    {
        // Poèetno stanje: inventory je skriven
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        // Prikazivanje/skrivanje inventory-ja na pritisak tipke E
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }

        // Ako je inventory aktivan, postavi kursor vidljivim i omoguæi slobodno kretanje kursora
        if (isInventoryVisible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else // Inaèe, sakrij kursor i zakljuèaj ga u središte ekrana
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Metoda za prikazivanje/skrivanje inventory
    void ToggleInventory()
    {
        // Ako je inventory aktivan, skrij ga ako nije, prikaži ga
        inventoryUI.SetActive(!inventoryUI.activeSelf);

        // Ažuriraj stanje prikaza inventory
        isInventoryVisible = inventoryUI.activeSelf;
    }
}
