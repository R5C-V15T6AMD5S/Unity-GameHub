using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject attackArea;

    [SerializeField]
    private GameObject weapon;

    private const float TimeToAttack = 1f; //set to player attack speed
    private float attackTimer = 0.5f;   //ovo spojiti sa ovim brojem gore

    private void Start()
    {
        if (weapon == null)
        {
            weapon = transform.GetChild(0).gameObject;
        }
        attackArea.SetActive(false);
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeToAttack);
            StartAttack();
            yield return new WaitForSeconds(attackTimer); // Adjust this delay as needed
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