using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int dmg;
    
    private float movementSpeed;
    
    [SerializeField] private EnemyData data;
    
    private float damageTimer = 0f;
    
    private const float TimeToDamage = 0.2f; // Set this to the desired damage interval
    

    private GameObject _player;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        SetEnemyValues();
    }
    
    private void Update()
    {
        Swarm();
        damageTimer += Time.deltaTime;
    }

    private void SetEnemyValues()   //Instantiate enemy values
    {
        GetComponent<Health>().SetHealth(data.hp, data.hp);
        dmg = data.dmg;
        movementSpeed = data.movementSpeed;
    }

    private void Swarm()    //Enemies move towards player location
    {
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, movementSpeed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D col)  //when collision boxes collide damage is dealt
    {
        if (col.GetComponent<Health>() == null) 
        {
            return;
        }
        if (!(damageTimer >= TimeToDamage)) return;
        col.GetComponent<Health>().Damage(dmg);
        Debug.Log("Enemy dealt dmg");
        damageTimer = 0f;
    }
}
