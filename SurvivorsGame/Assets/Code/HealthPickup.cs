using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.gameObject.CompareTag("Player")) return;
        other.gameObject.GetComponent<Health>().Heal(10);
        Destroy(gameObject); 
    }
}
