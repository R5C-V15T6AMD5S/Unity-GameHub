using System.Collections;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private float basicSpawnerInterval = 3.5f;
    
    public float safeRadius = 8f;
    
    private GameObject _player;
    private void Start()
    {
        StartCoroutine(SpawnEnemy(basicSpawnerInterval, enemyPrefab));
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)    //recursive method that is responsible for spawning enemies
    {
        yield return new WaitForSeconds(interval);
        Debug.Log("Enemy spawned");
        _player = GameObject.FindGameObjectWithTag("Player");
        Vector3 enemySpawnLocation;
        var position = _player.transform.position;
        do
        {
            enemySpawnLocation = new Vector3(Random.Range(-15f, 15f) + position.x, Random.Range(16f, 16f) + position.y, 0);
        }
        while (Vector3.Distance(enemySpawnLocation, position) <= safeRadius);
        var newEnemy = Instantiate(enemy, enemySpawnLocation, Quaternion.identity);
        StartCoroutine(SpawnEnemy(interval, enemy));
    }
}
