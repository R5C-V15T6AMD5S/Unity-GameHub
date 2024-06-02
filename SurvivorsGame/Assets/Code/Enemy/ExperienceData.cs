using Code.Player;
using UnityEngine;

namespace Code.Enemy
{
    public class ExperienceData : MonoBehaviour
    {
        public int amount = 5;
    
        private GameObject _player;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D col)  //when player collides with exp exp is added up
        {
            if (!col.CompareTag("Player")) return;
            _player.GetComponent<PlayerLeveling>().ExpPickup(amount);
            Destroy(gameObject);
        }
    }
}
