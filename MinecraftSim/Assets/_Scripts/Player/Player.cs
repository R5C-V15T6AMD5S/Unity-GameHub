using System;
using UnityEngine;

// Klasa koja služi za zdravlje igraèa
public class Player : MonoBehaviour
{
    // Definiranje maksimalnog zdravlja i trenutnog zdravlja
    public int MaxHealth = 100;
    private int currentHealth;

    // Dogaðaj koji se poziva kada se promijeni zdravlje igraèa.
    public static event Action<int> OnHealthChanged;

    // Metoda koja se poziva pri pokretanju igraèa.
    // Postavljanje treutnog zdravlja
    private void Start()
    {
        currentHealth = MaxHealth;
    }

    // Metoda koja se poziva pri svakom frame-u provjerava ako je
    // pritisnut space ukoliko je smanjuje se zdravlje igraèa
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    // Metoda za primanje štete igraèa u kojoj se poziva
    // dogaðaj za promjenu zdravlja na health baru
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);
    }
}
