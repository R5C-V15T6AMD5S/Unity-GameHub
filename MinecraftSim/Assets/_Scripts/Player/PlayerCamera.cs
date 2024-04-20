using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klasa za kontroliranje kamere
// odnosno pogleda igra�a
public class PlayerCamera : MonoBehaviour
{
    // Varijabla za osjetljivost pomicanja mi�a
    [SerializeField]
    private float sensitivity = 300f;
    // Referenca na igra�a
    [SerializeField]
    private Transform playerBody;
    // Referenca na skriptu za upravljanje igra�a
    [SerializeField]
    private PlayerInput playerInput;
    // Referenca na inventory igra�a
    private InventoryController inventoryController;

    // Varijabla za vertikalnu rotaciju kamere
    float verticalRotation = 0f;

    // Metoda koja se prva izvr�ava, povezuje skriptu za upravljanje igra�a
    // i tra�i postoji li inventoryController u sceni
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        inventoryController = FindObjectOfType<InventoryController>();
    }

    // Zaklju�avanje kursora mi�a
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Metoda koja se poziva svaki frame, rotira kameru ovisno o pomicanju mi�a
    void Update()
    {
        // Ukoliko je inventory otvoren, prekidanje ove metode
        if (inventoryController != null && inventoryController.inventoryUI.activeSelf) return; 

        // Izra�un pomaka mi�a na osnovu osjetljivosti i vremena
        float mouseX = playerInput.MousePosition.x * sensitivity * Time.deltaTime;
        float mouseY = playerInput.MousePosition.y * sensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
