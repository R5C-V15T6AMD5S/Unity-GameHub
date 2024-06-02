using UnityEngine;

namespace Code
{
    public class HealthPickupSpawner : MonoBehaviour
    {
        public GameObject healthPickupPrefab; // Assign your HealthPickup prefab in the Inspector
        public int numberOfPickups = 50; // Number of pickups to spawn
        public float minX = -10f; // Define the boundaries of the spawning area
        public float maxX = 10f;
        public float minY = -10f;
        public float maxY = 10f;

        private void Start()
        {
            for (var i = 0; i < numberOfPickups; i++)
            {
                var randomX = Random.Range(minX, maxX);
                var randomY = Random.Range(minY, maxY);
                var randomPosition = new Vector2(randomX, randomY);

                Instantiate(healthPickupPrefab, randomPosition, Quaternion.identity);
            }
        }
    }
}