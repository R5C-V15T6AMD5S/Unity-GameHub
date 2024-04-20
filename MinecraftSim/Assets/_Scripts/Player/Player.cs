using System;
using UnityEngine;

// Klasa koja slu�i za zdravlje igra�a
public class Player : MonoBehaviour
{
    // Definiranje maksimalnog zdravlja i trenutnog zdravlja
    public int MaxHealth = 100;
    private int currentHealth;

    // Doga�aj koji se poziva kada se promijeni zdravlje igra�a.
    public static event Action<int> OnHealthChanged;

    // Metoda koja se poziva pri pokretanju igra�a.
    // Postavljanje treutnog zdravlja
    private void Start()
    {
        currentHealth = MaxHealth;
    }

    // Metoda koja se poziva pri svakom frame-u provjerava ako je
    // pritisnut space ukoliko je smanjuje se zdravlje igra�a
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    // Metoda za primanje �tete igra�a u kojoj se poziva
    // doga�aj za promjenu zdravlja na health baru
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);
    }
}
