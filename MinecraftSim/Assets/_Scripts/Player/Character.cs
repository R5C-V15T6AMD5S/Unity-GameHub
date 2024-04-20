using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klasa koja upravlja pona�anjem igra�a,
// kretanjem, animacijama
public class Character : MonoBehaviour
{
    // Definiranje reference na glavnu kameru
    [SerializeField]
    private Camera mainCamera;
    // Definiranje reference za upravljanje igra�a
    [SerializeField]
    private PlayerInput playerInput;
    // Definiranje reference za kretanje igra�a
    [SerializeField]
    private PlayerMovement playerMovement;

    // Definiranje duljine raycast-a za interakciju s objektima 
    public float interactionRayLength = 5;

    // Definiranje sloja na kojem se nalazi igra�
    public LayerMask groundMask;
    // Definiranje varijable za letenje
    public bool fly = false;

    // Definiranje reference na animatora 
    public Animator animator;

    // Definiranje varijable koja ozna�ava je li igra� u procesu �ekanja
    bool isWaiting = false;

    // Metoda koja se prva poziva kada se objekt u�ita u memoriju,
    // provjerava je li postavljena glavna kamera i dohva�a i povezuje komponente
    // za upravljanje i kretanje igra�a
    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Metoda koja se poziva jedanput, pretpla�uje
    // se na doga�aje iz PlayerInput-a
    private void Start()
    {
        playerInput.OnMouseClick += HandleMouseClick;
        playerInput.OnFly += HandleFlyClick;
    }
    // Metoda koja se izvodi kada se pritisne tipka V
    // postavlja igra�a u stanje letenja odnosno u normalno stanje
    // ovisno o prija�njem stanju
    private void HandleFlyClick()
    {
        fly = !fly;
    }

    // Metoda koja se poziva svaki frame, provjerava ako je igra� u
    // stanju letenja ukoliko je, postavlja animacije i poziva metodu za letenje,
    // ukoliko je stanje igra�a normalno postavlja animator na tlo 
    void Update()
    {
        if (fly)
        {
            animator.SetFloat("speed", 0);
            animator.SetBool("isGrounded", false);
            animator.ResetTrigger("jump");
            playerMovement.Fly(playerInput.MovementInput, playerInput.IsJumping, playerInput.RunningPressed);
        }
        else 
        {
           animator.SetBool("isGrounded", playerMovement.IsGrounded);
           // provjera je li pritisnuta tipka Space 
           if (playerMovement.IsGrounded && playerInput.IsJumping && isWaiting == false)
           {
                // Ukoliko je pritisnuta tipka za skakanje pokre�e se animacija za skakanje
                // pokre�e se odre�eno vrijeme �ekanja prije nego se animacija skoka resetira 
                animator.SetTrigger("jump");
                isWaiting = true;
                StopAllCoroutines();
                StartCoroutine(ResetWaiting());
           } 
           // Postavlja vrijednost animatora za brzinu kretanja
           animator.SetFloat("speed", playerInput.MovementInput.magnitude);
           // Obrada gravitacije na igra�a
           playerMovement.HandleGravity(playerInput.IsJumping);
           // nakon �to se skok zavr�i pokre�e se metoda za hodanje
           playerMovement.Walk(playerInput.MovementInput, playerInput.RunningPressed);
        }
    }

    // Korutina za resetiranje �ekanja nakon skoka, �ekanje 0.1s
    // prije resetiranja trigger-a za skok i postavljanje zastavice �ekanja na false
    IEnumerator ResetWaiting()
    {
        yield return new WaitForSeconds(0.1f);
        animator.ResetTrigger("jump");
        isWaiting = false;
    }

    // Metoda koja �e slu�it za kopanje blokova
    private void HandleMouseClick()
    {
        
    }
}
