using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int hp;
    
    [SerializeField] private GameObject exp;

    private int _maxHealth = 100;

    private GameObject _hud;
    
    private void Start()
    {
        _hud = GameObject.FindGameObjectWithTag("HUD");
    }

    public void SetHealth(int maxHealth, int health)    //instantiate values
    {
        _maxHealth = maxHealth;
        hp = health;
    }
    public void Damage(int amount)  //method responsible for damage
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Can't have negative damage!");
        }
        hp -= amount;
        _hud.GetComponent<HUDManager>().ChangeHealthCount();
        if (hp > 0) return;
        Die();
    }

    public void Heal(int amount)    //method responsible for healing
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Heal can't be negative!");
        }

        if (hp + amount > _maxHealth)
        {
            hp = _maxHealth;
        }
        else
        {
            hp += amount;
        }
    }

    private void Die()  //method responsible for death and exp generation
    {
        Debug.Log("Died!");
        _hud.GetComponent<HUDManager>().ChangeKillCount();
        if (exp != null)
        {
            var newExp = Instantiate(exp, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
