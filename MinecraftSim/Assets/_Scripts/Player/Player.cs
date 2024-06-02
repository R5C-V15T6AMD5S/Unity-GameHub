using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int MaxHealth = 100;
    public int currentHealth;

    public static event Action<int> OnHealthChanged;

    private bool isFalling = false;
    private float startFallHeight;
    private float fallThreshold = 10.0f; // Minimalna visina pada koja uzrokuje �tetu
    private float damageMultiplier = 2.0f; // Koliko �tete po metru pada
    private float previousYPosition;

    private void Start()
    {
        currentHealth = MaxHealth;
        previousYPosition = transform.position.y;
    }

    public void Update()
    {
        // Pra�enje pada
        if (transform.position.y < previousYPosition)
        {
            if (!isFalling)
            {
                isFalling = true;
                startFallHeight = previousYPosition;
            }
        }
        else
        {
            if (isFalling)
            {
                isFalling = false;
                float fallDistance = startFallHeight - transform.position.y;
                if (fallDistance > fallThreshold)
                {
                    int fallDamage = Mathf.FloorToInt((fallDistance - fallThreshold) * damageMultiplier);
                    
                    TakeDamage(fallDamage);
                }
            }
        }

        // A�uriranje prethodne Y pozicije
        previousYPosition = transform.position.y;
        Debug.Log("NESTO");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isFalling)
            {
                isFalling = false;
                float fallDistance = startFallHeight - transform.position.y;
                if (fallDistance > fallThreshold)
                {
                    int fallDamage = Mathf.FloorToInt((fallDistance - fallThreshold) * damageMultiplier);
                    TakeDamage(fallDamage);
                }
            }
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        OnHealthChanged?.Invoke(currentHealth);
        Debug.Log("Current Health: " + currentHealth);
    }
}
