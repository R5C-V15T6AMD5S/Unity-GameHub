using Code.Player;
using UnityEngine;

namespace Code
{
    public class RewardBox : MonoBehaviour
    {
        [SerializeField]
        public GameObject lvlUpMenuUI;
    
        private void Start()    //grabs the lvlUpMenuUI from the player
        {
            lvlUpMenuUI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLeveling>().lvlUpMenuUI;
        }
        private void OnTriggerEnter2D(Collider2D col) //opens the lvlUpMenuUI when the player collides with the reward box
        {
            if (!col.gameObject.CompareTag("Player")) return;
            lvlUpMenuUI.SetActive(true);
            Destroy(gameObject);
            Time.timeScale = 0f;
        }
    }
}
