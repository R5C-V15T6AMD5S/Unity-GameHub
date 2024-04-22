using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] private int dmg;
    

    private void OnTriggerEnter2D(Collider2D collider)  //collision box for player weapon
    {
        if (collider.GetComponent<Health>() == null) return;
        var hp = collider.GetComponent<Health>();
        Debug.Log("Player dealt dmg");
        hp.Damage(dmg);
        if (hp.hp <= 0)
        {
            //game over
        }
    }
}
