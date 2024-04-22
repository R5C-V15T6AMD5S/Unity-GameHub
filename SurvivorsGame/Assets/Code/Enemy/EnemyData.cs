using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    //values for enemy data
    public int hp;
    
    public int dmg;
    
    public float movementSpeed;
    
}
