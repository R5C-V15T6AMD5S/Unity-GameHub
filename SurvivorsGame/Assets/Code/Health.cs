using Level.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class Health : MonoBehaviour
    {
        [SerializeField] 
        public int hp;
    
        [SerializeField] 
        private GameObject exp;

        public int maxHealth = 100;
    
        public GameObject rewardBoxPrefab;
        
        public float spawnChance = 0.05f; // 5% spawn chance
    
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
            if (!gameObject.CompareTag("Player"))
            {
                _hud.GetComponent<HUDManager>().ChangeKillCount();
            }
            else if (gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene("GameOver");
            }
            if (exp != null)
            {
                Instantiate(exp, transform.position, Quaternion.identity);
                // Generate a random number between 0 and 1
                var randomNumber = Random.value;

                // If the random number is less than or equal to the spawn chance, spawn the reward box
                if (randomNumber <= spawnChance)
                {
                    Instantiate(rewardBoxPrefab, transform.position, Quaternion.identity);
                }
            }
            Destroy(gameObject);
        }
    }
}
