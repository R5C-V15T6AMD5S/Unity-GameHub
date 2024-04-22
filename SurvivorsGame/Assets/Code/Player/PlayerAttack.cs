using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject _attackArea;

    [SerializeField] private GameObject weapon;

    private bool _attacking = false;

    private const float TimeToAttack = 0.5f;

    private float _timer = 0f;
    
    private void Start()
    {
        _attackArea = weapon == null ? transform.GetChild(0).gameObject : weapon;
        //takes child as weapon tldr:should probably make it more scaleable to account for more weapons
    }
    private void Update()   //attacking loop auto activates on a set interval
    {
        Attack();
        if (!_attacking) return;
        _timer += Time.deltaTime;
        if (!(_timer >= TimeToAttack)) return;
        _timer = 0;
        _attacking = false;
        _attackArea.SetActive(_attacking);
    }

    private void Attack()   //hitbox is activated
    {
        _attacking = true;
        _attackArea.SetActive(_attacking);
    }
}
