using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] 
    public int hp;
    
    [SerializeField] 
    private GameObject exp;

    public int maxHealth = 100;
    
    private GameObject _hud;
    
    private void Start()
    {
        _hud = GameObject.FindGameObjectWithTag("HUD");
    }

    public void SetHealth(int maxHp, int health)    //instantiate values
    {
        maxHealth = maxHp;
        hp = health;
        if (gameObject.CompareTag("Player"))
        {
            _hud.GetComponent<HUDManager>().ChangeHealthCount();
        }
    }
    public void Damage(int amount)  //method responsible for damage
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Can't have negative damage!");
        }
        hp -= amount;
        if (gameObject.CompareTag("Player"))
        {
            _hud.GetComponent<HUDManager>().ChangeHealthCount();
        }
        if (hp > 0) return;
        Die();
    }

    public void Heal(int amount)    //method responsible for healing
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Heal can't be negative!");
        }

        if (hp + amount > maxHealth)
        {
            hp = maxHealth;
        }
        else
        {
            hp += amount;
        }
        if (gameObject.CompareTag("Player"))
        {
            _hud.GetComponent<HUDManager>().ChangeHealthCount();
        }
    }

    private void Die()  //method responsible for death and exp generation
    {
        Debug.Log("Died!");
        if (gameObject.CompareTag("Player"))
        {
            _hud.GetComponent<HUDManager>().ChangeKillCount();
        }
        if (exp != null)
        {
            Instantiate(exp, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
