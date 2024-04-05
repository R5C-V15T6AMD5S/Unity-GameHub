using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private PlayerMovement playerMovement;

    public float interactionRayLenght = 5;

    public LayerMask groundMask;
    public bool fly = false;

    public Animator animator;

    bool isWaiting = false;

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        playerInput.OnMouseClick += HandleMouseClick;
        playerInput.OnFly += HandleFlyClick;
    }

    private void HandleFlyClick()
    {
        fly = !fly;
    }

    private void Update()
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
            if (playerMovement.IsGrounded && playerInput.IsJumping && isWaiting == false)
            {
                animator.SetTrigger("jump");
                isWaiting = true;
                StartCoroutine(ResetWaiting());
            }

            // Dodajte kontrolu za pokretanje animacije hodanja tek kad se pritisne tipka W, A, S ili D
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                animator.SetFloat("speed", playerInput.MovementInput.magnitude);
                playerMovement.Walk(playerInput.MovementInput, playerInput.RunningPressed);
            }
            else
            {
                animator.SetFloat("speed", 0f);
            }

            playerMovement.HandleGravity(playerInput.IsJumping);
        }
    }

    IEnumerator ResetWaiting()
    {
        yield return new WaitForSeconds(0.1f);
        animator.ResetTrigger("jump");
        isWaiting = false;
    }

    private void HandleMouseClick()
    {
        // Implementirajte funkcionalnost kada se pritisne miš
    }
}
