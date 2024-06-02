using UnityEngine;

namespace Code
{
    public class HealthPickup : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col) //used for healing the player
        {
            if (!col.gameObject.CompareTag("Player")) return;
            col.gameObject.GetComponent<Health>().Heal(10);
            Destroy(gameObject); 
        }
    }
}
