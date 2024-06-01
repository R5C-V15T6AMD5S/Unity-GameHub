using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBox : MonoBehaviour
{
    public GameObject lvlUpMenuUI;
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        lvlUpMenuUI = GameObject.FindGameObjectWithTag("LvlUpMenu");
        if (!other.gameObject.CompareTag("Player")) return;
        Destroy(gameObject);
        lvlUpMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
