using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klasa za upravljanje kretnjom igra�a
public class PlayerMovement : MonoBehaviour
{
    // Referenca na Kontroler igra�a
    [SerializeField]
    private CharacterController controller;
    // Definiranje brzine igra�a kad hoda i kad tr�i
    [SerializeField]
    private float playerSpeed = 5.0f, playerRunSpeed = 8;
    // Definiranje visine skoka igra�a
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    // Definiranje gravitacije
    private float gravityValue = -9.81f;
    // Definiranje brzine kod letenja
    [SerializeField]
    private float flySpeed = 2;

    // Vektor za predstavljanje brzine igra�a
    private Vector3 playerVelocity;

    // Parametri za provjeru je li igra�  na tlu
    [Header("Grounded check parameters:")]
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float rayDistance = 1;
    [field: SerializeField]
    public bool IsGrounded { get; private set; }

    // Definiranje varijable za inventory
    private InventoryController inventoryController;

    // Metoda koja povezuje kontroler igra�a i tra�i
    // postoji li kontroler Inventory-ja
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inventoryController = FindObjectOfType<InventoryController>();
    }
    // Metoda koja odre�uje smjer kretanja igra�a
    private Vector3 GetMovementDirection(Vector3 movementInput)
    {
        return transform.right * movementInput.x + transform.forward * movementInput.z;
    }

    // Metoda koja slu�i za let igra�a
    public void Fly(Vector3 movementInput, bool ascendInput, bool descendInput)
    {
        Vector3 movementDirection = GetMovementDirection(movementInput);

        if (ascendInput)
        {
            movementDirection += Vector3.up * flySpeed;
        }
        else if (descendInput)
        {
            movementDirection -= Vector3.up * flySpeed;
        }
        controller.Move(movementDirection * playerSpeed * Time.deltaTime);
    }
    // Metoda koja slu�i za kretanje igra�a, za hodanje i tr�anje,
    // brzina ovisi je li pritisnuta Shift tipka
    public void Walk(Vector3 movementInput, bool runningInput)
    {
        if(inventoryController != null && inventoryController.inventoryUI.activeSelf)
        {
            return ;
        }

        Vector3 movementDirection = GetMovementDirection(movementInput);
        float speed = runningInput ? playerRunSpeed : playerSpeed;
        controller.Move(movementDirection * Time.deltaTime * speed);
    }
    // Metoda koja upravlja gravitacijom
    public void HandleGravity(bool isJumping)
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        if (isJumping && IsGrounded && !(inventoryController != null && inventoryController.inventoryUI.activeSelf)) AddJumpForce();
        ApplyGravityForce();
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // Dodavanje silu skoka
    private void AddJumpForce()
    {
        //playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        playerVelocity.y = jumpHeight;
    }
    // Metoda za izra�unvanje i primjenjivanje gravitacijsku silu
    private void ApplyGravityForce()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        playerVelocity.y = Mathf.Clamp(playerVelocity.y, gravityValue, 10);
    }
    // Provjera da li je igra� na tlu
    private void Update()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundMask);
    }
    // Metoda koja crta vizualni prikaz raycasta za provjeru tla u Unity Editoru
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * rayDistance);
    }


}
