using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klasa PlayerInput služi za upravljenje
// kretanja igraèa ovisno o pritisnutoj tipki
public class PlayerInput : MonoBehaviour
{
    public event Action OnMouseClick, OnMouseClickBuild, OnFly; // Definiranje dogaðaja
    public bool RunningPressed { get; private set; } // Varijabla koja prati ako je pritisnuta tipka za trèanje

    public Vector3 MovementInput { get; private set; } // Varijabla koja prati kretanje igraèa

    public Vector2 MousePosition { get; private set; } // Varijabla koja prati položaj miša

    public bool IsJumping { get; private set; } // Varijabla koja prati aok je pritisnuta tipka za skok igraèa

    // Metoda koja se poziva svaki frame
    void Update()
    {
        GetMouseClick();
        GetMousePosition();
        GetMovementInput();
        GetJumpInput();
        GetRunInput();
        GetFlyInput();
    }

    // Metoda za provjeru ako je pritisnuta tipka za let
    // Ukoliko je pritisnuta tipka V priziva se dogaðaj OnFly
    private void GetFlyInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnFly?.Invoke();
        }
    }

    // Provjera ako je pritisnuta tipka Shift, ako je onda se varijabla RunningPressed
    // postavi na true, a drugim sluèajevima RunningPressed je jednak false
    private void GetRunInput()
    {
        RunningPressed = Input.GetKey(KeyCode.LeftShift);
    }

    // Provjera ako je pritisnuta tipka za skok, Unity veæ ima preddefinirane tipke za Skok (Space),
    // ukoliko je pritisnuta IsJumping se postavi na true
    private void GetJumpInput()
    {
        IsJumping = Input.GetButton("Jump");
    }

    // Metoda za provjeru ulaza za kretanje igraèa,
    // postavlja se MovementInput na temelju pritisnutih tipki za kretanje (tipke W, A, S i D)
    private void GetMovementInput()
    {
        MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }
    // Metoda za provjeru položaja miša,
    // postavlja se MousePosition na temelju položaja miša
    private void GetMousePosition()
    {
        MousePosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
    
    // Provjera za klik miša, ukoliko je kliknuta lijeva tipka miša
    // poziva se dogaðaj OnMouseClick
    private void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClick?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnMouseClickBuild?.Invoke();
        }
    }
}
