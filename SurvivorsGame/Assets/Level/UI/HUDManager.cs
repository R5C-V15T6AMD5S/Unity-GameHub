using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Text timeCounter;

    private const string TimeText = "Time: ";

    private const string HealthText = "HP: ";

    private const string LvlText = "LV: ";

    [SerializeField] private Text hpCount;
    
    [SerializeField] private Text lvlCount;
    
    private float _timeInLevel;

    private GameObject _player;
    
    private void Start()    //fetches starting values of player
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        hpCount.text = HealthText + _player.GetComponent<Health>().hp;
        lvlCount.text = LvlText + _player.GetComponent<PlayerLeveling>().lvl;
    }

    private void Update()   //tracks time
    {
        _timeInLevel += Time.deltaTime;
        timeCounter.text = TimeText + Math.Round(_timeInLevel, 0);
    }

    public void ChangeHealthCount() //changes health value when damage is taken
    {
        hpCount.text = HealthText + _player.GetComponent<Health>().hp;
    }

    public void LevelUp()   //changes level value when player levels up
    {
        Debug.Log("Leveled up!");
        lvlCount.text = LvlText + _player.GetComponent<PlayerLeveling>().lvl;
    }
}
