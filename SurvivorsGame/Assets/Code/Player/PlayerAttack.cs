using System.Collections;
using UnityEngine;

namespace Code.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        private GameObject attackArea;

        [SerializeField]
        private GameObject weapon;

        private float _timeToAttack = 1f; //set to player attack speed
        private const float AttackTimer = 0.2f; //set to weapon active time

        private void Start()
        {
            if (weapon == null)
            {
                weapon = transform.GetChild(0).gameObject;
            }
            attackArea.SetActive(false);
            StartCoroutine(AttackRoutine());
        }
    
        public void IncreaseAttackSpeed(float attackSpeed) //increase attack speed by a certain amount
        {
            _timeToAttack -= attackSpeed;
        }

        private IEnumerator AttackRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_timeToAttack);
                StartAttack();
                yield return new WaitForSeconds(AttackTimer);
                EndAttack();
            }
        }

        private void StartAttack()
        {
            attackArea.SetActive(true);
            // Process damage here
        }

        private void EndAttack()
        {
            attackArea.SetActive(false);
        }
    }
}