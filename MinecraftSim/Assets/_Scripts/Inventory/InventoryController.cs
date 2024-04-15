using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Referenca na UI element inventara
    public GameObject inventoryUI;

    // Varijabla za pra�enje stanja prikaza inventara
    private bool isInventoryVisible = false;

    void Start()
    {
        // Po�etno stanje: inventar je skriven
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        // Prikazivanje/skrivanje inventara na pritisak tipke E
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }

        // Ako je inventar aktivan, postavi kursor vidljivim i omogu�i slobodno kretanje kursora
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

    // Metoda za prikazivanje/skrivanje inventara
    void ToggleInventory()
    {
        // Ako je inventar aktivan, skrij ga; ako nije, prika�i ga
        inventoryUI.SetActive(!inventoryUI.activeSelf);

        // A�uriraj stanje prikaza inventara
        isInventoryVisible = inventoryUI.activeSelf;
    }
}
