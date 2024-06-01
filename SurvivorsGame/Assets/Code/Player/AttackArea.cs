using System;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int _dmg = 5;

    public void IncreaseDamage(int damage)   //instantiate damage value
    {
        _dmg += damage;
    }

    private void OnTriggerEnter2D(Collider2D col)  //collision box for player weapon
    {
        if (col.GetComponent<Health>() == null) return;
        var hp = col.GetComponent<Health>();
        Debug.Log("Player dealt dmg");
        hp.Damage(_dmg);
    }
}
