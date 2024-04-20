using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klasa koja upravlja ponašanjem igraèa,
// kretanjem, animacijama
public class Character : MonoBehaviour
{
    // Definiranje reference na glavnu kameru
    [SerializeField]
    private Camera mainCamera;
    // Definiranje reference za upravljanje igraèa
    [SerializeField]
    private PlayerInput playerInput;
    // Definiranje reference za kretanje igraèa
    [SerializeField]
    private PlayerMovement playerMovement;

    // Definiranje duljine raycast-a za interakciju s objektima 
    public float interactionRayLength = 5;

    // Definiranje sloja na kojem se nalazi igraè
    public LayerMask groundMask;
    // Definiranje varijable za letenje
    public bool fly = false;

    // Definiranje reference na animatora 
    public Animator animator;

    // Definiranje varijable koja oznaèava je li igraè u procesu èekanja
    bool isWaiting = false;

    // Metoda koja se prva poziva kada se objekt uèita u memoriju,
    // provjerava je li postavljena glavna kamera i dohvaæa i povezuje komponente
    // za upravljanje i kretanje igraèa
    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Metoda koja se poziva jedanput, pretplaæuje
    // se na dogaðaje iz PlayerInput-a
    private void Start()
    {
        playerInput.OnMouseClick += HandleMouseClick;
        playerInput.OnFly += HandleFlyClick;
    }
    // Metoda koja se izvodi kada se pritisne tipka V
    // postavlja igraèa u stanje letenja odnosno u normalno stanje
    // ovisno o prijašnjem stanju
    private void HandleFlyClick()
    {
        fly = !fly;
    }

    // Metoda koja se poziva svaki frame, provjerava ako je igraè u
    // stanju letenja ukoliko je, postavlja animacije i poziva metodu za letenje,
    // ukoliko je stanje igraèa normalno postavlja animator na tlo 
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
                // Ukoliko je pritisnuta tipka za skakanje pokreæe se animacija za skakanje
                // pokreæe se odreðeno vrijeme èekanja prije nego se animacija skoka resetira 
                animator.SetTrigger("jump");
                isWaiting = true;
                StopAllCoroutines();
                StartCoroutine(ResetWaiting());
           } 
           // Postavlja vrijednost animatora za brzinu kretanja
           animator.SetFloat("speed", playerInput.MovementInput.magnitude);
           // Obrada gravitacije na igraèa
           playerMovement.HandleGravity(playerInput.IsJumping);
           // nakon što se skok završi pokreæe se metoda za hodanje
           playerMovement.Walk(playerInput.MovementInput, playerInput.RunningPressed);
        }
    }

    // Korutina za resetiranje èekanja nakon skoka, èekanje 0.1s
    // prije resetiranja trigger-a za skok i postavljanje zastavice èekanja na false
    IEnumerator ResetWaiting()
    {
        yield return new WaitForSeconds(0.1f);
        animator.ResetTrigger("jump");
        isWaiting = false;
    }

    // Metoda koja æe služit za kopanje blokova
    private void HandleMouseClick()
    {
        
    }
}
