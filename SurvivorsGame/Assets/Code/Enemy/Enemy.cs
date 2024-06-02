using UnityEngine;

namespace Code.Enemy
{
    public class Enemy : MonoBehaviour
    {
        private int _dmg;
    
        private float _movementSpeed;
    
        [SerializeField] private EnemyData data;
    
        private float _damageTimer;
    
        private const float TimeToDamage = 0.2f; //Time between damage dealt when collision occurs
    

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

        private void SetEnemyValues()   //Instantiate enemy values
        {
            GetComponent<Health>().SetHealth(data.hp, data.hp);
            _dmg = data.dmg;
            _movementSpeed = data.movementSpeed;
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
            Debug.Log("Enemy dealt dmg");
            _damageTimer = 0f;
        }
    }
}
