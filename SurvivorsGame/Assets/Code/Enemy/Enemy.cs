using System.Collections;
using Code.Player;
using UnityEngine;

namespace Code.Enemy
{
    public class Enemy : MonoBehaviour
    {
        private int _dmg;
    
        private float _movementSpeed;
    
        [SerializeField] private EnemyData data;
    
        private float _damageTimer;
    
        private const float TimeToDamage = 0.5f; //Time between damage dealt when collision occurs
    

        private GameObject _player;
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            SetEnemyValues();
        }
    
        private void Update()
        {
            Swarm();
            _damageTimer += Time.deltaTime;
        }

        private void SetEnemyValues()   //Instantiate enemy values from prefab
        {
            Debug.Log("Setting enemy values");
            var randomFactor = Random.Range(0.5f, 1.5f);
            _movementSpeed = data.movementSpeed * randomFactor;
            var playerLevel = _player.GetComponent<PlayerLeveling>().lvl;
            var timePassed = Time.time;
            var scalingFactor = Mathf.Log(playerLevel * timePassed + 1);    //scaling factor for enemy health and damage
            var enemyHealth = (int)(data.hp * scalingFactor * randomFactor);
            GetComponent<Health>().SetHealth((int) (enemyHealth * randomFactor),(int) (enemyHealth * randomFactor));
            _dmg = (int) (data.dmg * scalingFactor / 2);
        } 

        private void Swarm()    //Enemies move towards player location
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _movementSpeed * Time.deltaTime);
        }

        private void OnTriggerStay2D(Collider2D col)  //when collision boxes collide damage is dealt
        {
            if (col.GetComponent<Health>() == null) 
            {
                return;
            }
            if (!(_damageTimer >= TimeToDamage)) return;
            col.GetComponent<Health>().Damage(_dmg);
            var spriteRenderer = col.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;
            // Save the original color
            var originalColor = spriteRenderer.color;

            // Change the color of the sprite
            spriteRenderer.color = Color.red; // Change this to the color you want
            
            StartCoroutine(RevertColorAfterMilliseconds(spriteRenderer, originalColor, 200));
            Debug.Log("Enemy dealt dmg");
            _damageTimer = 0f;
        }
        
        private static IEnumerator RevertColorAfterMilliseconds(SpriteRenderer spriteRenderer, Color originalColor, int milliseconds)
        {
            // Wait for the specified number of milliseconds
            yield return new WaitForSeconds(milliseconds / 1000f);
            // Revert the color
            try
            {
                spriteRenderer.color = originalColor;
            }
            catch (MissingReferenceException e)
            {
            }
        }

        public int GetDmg()
        {
            return _dmg;
        }
    }
}
