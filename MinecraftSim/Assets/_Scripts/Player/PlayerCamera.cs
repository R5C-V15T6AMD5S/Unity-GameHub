using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klasa za kontroliranje kamere
// odnosno pogleda igraèa
public class PlayerCamera : MonoBehaviour
{
    // Varijabla za osjetljivost pomicanja miša
    [SerializeField]
    private float sensitivity = 300f;
    // Referenca na igraèa
    [SerializeField]
    private Transform playerBody;
    // Referenca na skriptu za upravljanje igraèa
    [SerializeField]
    private PlayerInput playerInput;
    // Referenca na inventory igraèa
    private InventoryController inventoryController;

    // Varijabla za vertikalnu rotaciju kamere
    float verticalRotation = 0f;

    // Metoda koja se prva izvršava, povezuje skriptu za upravljanje igraèa
    // i traži postoji li inventoryController u sceni
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        inventoryController = FindObjectOfType<InventoryController>();
    }

    // Zakljuèavanje kursora miša
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Metoda koja se poziva svaki frame, rotira kameru ovisno o pomicanju miša
    void Update()
    {
        // Ukoliko je inventory otvoren, prekidanje ove metode
        if (inventoryController != null && inventoryController.inventoryUI.activeSelf) return; 

        // Izraèun pomaka miša na osnovu osjetljivosti i vremena
        float mouseX = playerInput.MousePosition.x * sensitivity * Time.deltaTime;
        float mouseY = playerInput.MousePosition.y * sensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
