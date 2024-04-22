using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int dmg = 5;
    
    [SerializeField] private float movementSpeed = 1.5f;
    
    [SerializeField] private EnemyData data;
    

    private GameObject _player;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        SetEnemyValues();
    }
    
    private void Update()
    {
        Swarm();
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

    private void OnTriggerEnter2D(Collider2D collider)  //when collision boxes collide damage is dealt
    {
        if (!collider.CompareTag("Player")) return;
        if (collider.GetComponent<Health>() == null) return;
        collider.GetComponent<Health>().Damage(dmg);
    }
}
