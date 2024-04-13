using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 300f;
    [SerializeField]
    private Transform playerBody;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private InventoryController inventoryController; // Dodana referenca na InventoryController

    float verticalRotation = 0f;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Provjera je li inventar aktivan; ako je, prekini izvršavanje ostatka metode
        if (inventoryController != null && inventoryController.inventoryUI.activeSelf)
        {
            return;
        }

        float mouseX = playerInput.MousePosition.x * sensitivity * Time.deltaTime;
        float mouseY = playerInput.MousePosition.y * sensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
