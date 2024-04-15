using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Referenca na UI element inventara
    public GameObject inventoryUI;

    // Varijabla za praæenje stanja prikaza inventara
    private bool isInventoryVisible = false;

    void Start()
    {
        // Poèetno stanje: inventar je skriven
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        // Prikazivanje/skrivanje inventara na pritisak tipke E
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }

        // Ako je inventar aktivan, postavi kursor vidljivim i omoguæi slobodno kretanje kursora
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

    // Metoda za prikazivanje/skrivanje inventara
    void ToggleInventory()
    {
        // Ako je inventar aktivan, skrij ga; ako nije, prikaži ga
        inventoryUI.SetActive(!inventoryUI.activeSelf);

        // Ažuriraj stanje prikaza inventara
        isInventoryVisible = inventoryUI.activeSelf;
    }
}
