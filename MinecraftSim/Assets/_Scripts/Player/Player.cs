using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int MaxHealth = 100;
    private int currentHealth;

    public static event Action<int> OnHealthChanged;

    private void Start()
    {
        currentHealth = MaxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);
    }
}
