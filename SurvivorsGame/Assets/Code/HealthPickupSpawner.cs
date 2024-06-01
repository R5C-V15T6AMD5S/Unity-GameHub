using UnityEngine;

public class HealthPickupSpawner : MonoBehaviour
{
    public GameObject healthPickupPrefab; // Assign your HealthPickup prefab in the Inspector
    public int numberOfPickups = 50; // Number of pickups to spawn
    public float minX = -10f; // Define the boundaries of the spawning area
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    void Start()
    {
        for (int i = 0; i < numberOfPickups; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            Instantiate(healthPickupPrefab, randomPosition, Quaternion.identity);
        }
    }
}