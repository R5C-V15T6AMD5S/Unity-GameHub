using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klasa za upravljanje kretnjom igraèa
public class PlayerMovement : MonoBehaviour
{
    // Referenca na Kontroler igraèa
    [SerializeField]
    private CharacterController controller;
    // Definiranje brzine igraèa kad hoda i kad trèi
    [SerializeField]
    private float playerSpeed = 5.0f, playerRunSpeed = 8;
    // Definiranje visine skoka igraèa
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    // Definiranje gravitacije
    private float gravityValue = -9.81f;
    // Definiranje brzine kod letenja
    [SerializeField]
    private float flySpeed = 2;

    // Vektor za predstavljanje brzine igraèa
    private Vector3 playerVelocity;

    // Parametri za provjeru je li igraè  na tlu
    [Header("Grounded check parameters:")]
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float rayDistance = 1;
    [field: SerializeField]
    public bool IsGrounded { get; private set; }

    // Definiranje varijable za inventory
    private InventoryController inventoryController;

    // Metoda koja povezuje kontroler igraèa i traži
    // postoji li kontroler Inventory-ja
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inventoryController = FindObjectOfType<InventoryController>();
    }
    // Metoda koja odreðuje smjer kretanja igraèa
    private Vector3 GetMovementDirection(Vector3 movementInput)
    {
        return transform.right * movementInput.x + transform.forward * movementInput.z;
    }

    // Metoda koja služi za let igraèa
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
    // Metoda koja služi za kretanje igraèa, za hodanje i trèanje,
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
    // Metoda za izraèunvanje i primjenjivanje gravitacijsku silu
    private void ApplyGravityForce()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        playerVelocity.y = Mathf.Clamp(playerVelocity.y, gravityValue, 10);
    }
    // Provjera da li je igraè na tlu
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
