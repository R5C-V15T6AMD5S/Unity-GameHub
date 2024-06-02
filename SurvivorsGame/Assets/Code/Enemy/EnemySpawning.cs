using System.Collections;
using Code.Player;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemySpawning : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;

        [SerializeField] private float basicSpawnerInterval = 3.5f;
    
        public float safeRadius = 15f;
    
        private GameObject _player;
        private void Start()   //initializing the player and starting the spawning process
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(SpawnEnemy(basicSpawnerInterval, enemyPrefab));
        }

        private IEnumerator SpawnEnemy(float interval, GameObject enemy)    //recursive method that is responsible for spawning enemies
        {
            yield return new WaitForSeconds(interval * 1 / (_player.GetComponent<PlayerLeveling>().lvl * 0.5f));
            Debug.Log("Enemy spawned");
            _player = GameObject.FindGameObjectWithTag("Player");
            Vector3 enemySpawnLocation;
            var position = _player.transform.position;
            do
            {
                enemySpawnLocation = new Vector3(Random.Range(-30f, 30f) + position.x, Random.Range(-30f, 30f) + position.y, 0);
            }
            while (Vector3.Distance(enemySpawnLocation, position) <= safeRadius);
            var newEnemy = Instantiate(enemy, enemySpawnLocation, Quaternion.identity);
            StartCoroutine(SpawnEnemy(interval, enemy));
        }
    }
}
