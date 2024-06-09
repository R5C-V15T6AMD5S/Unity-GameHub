using UnityEngine;

namespace Code.Player
{
    public class AttackArea : MonoBehaviour
    {
        private int _dmg = 10;

        public void IncreaseDamage(int damage)   //increase damage of player weapon
        {
            _dmg += damage;
        }

        private void OnTriggerEnter2D(Collider2D col)  //collision box for player weapon
        {
            if (col.GetComponent<Health>() == null) return;
            var hp = col.GetComponent<Health>();
            Debug.Log("Player dealt dmg: " + _dmg + "out of " + hp.hp);
            hp.Damage(_dmg);
        }
        
    }
}
