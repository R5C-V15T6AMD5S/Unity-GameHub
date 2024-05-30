using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klasa PlayerInput slu�i za upravljenje
// kretanja igra�a ovisno o pritisnutoj tipki
public class PlayerInput : MonoBehaviour
{
    public event Action OnMouseClick, OnMouseClickBuild, OnFly; // Definiranje doga�aja
    public bool RunningPressed { get; private set; } // Varijabla koja prati ako je pritisnuta tipka za tr�anje

    public Vector3 MovementInput { get; private set; } // Varijabla koja prati kretanje igra�a

    public Vector2 MousePosition { get; private set; } // Varijabla koja prati polo�aj mi�a

    public bool IsJumping { get; private set; } // Varijabla koja prati aok je pritisnuta tipka za skok igra�a

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
    // Ukoliko je pritisnuta tipka V priziva se doga�aj OnFly
    private void GetFlyInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnFly?.Invoke();
        }
    }

    // Provjera ako je pritisnuta tipka Shift, ako je onda se varijabla RunningPressed
    // postavi na true, a drugim slu�ajevima RunningPressed je jednak false
    private void GetRunInput()
    {
        RunningPressed = Input.GetKey(KeyCode.LeftShift);
    }

    // Provjera ako je pritisnuta tipka za skok, Unity ve� ima preddefinirane tipke za Skok (Space),
    // ukoliko je pritisnuta IsJumping se postavi na true
    private void GetJumpInput()
    {
        IsJumping = Input.GetButton("Jump");
    }

    // Metoda za provjeru ulaza za kretanje igra�a,
    // postavlja se MovementInput na temelju pritisnutih tipki za kretanje (tipke W, A, S i D)
    private void GetMovementInput()
    {
        MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }
    // Metoda za provjeru polo�aja mi�a,
    // postavlja se MousePosition na temelju polo�aja mi�a
    private void GetMousePosition()
    {
        MousePosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
    
    // Provjera za klik mi�a, ukoliko je kliknuta lijeva tipka mi�a
    // poziva se doga�aj OnMouseClick
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
