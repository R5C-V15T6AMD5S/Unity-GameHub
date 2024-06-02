using System;
using Code;
using Code.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Level.UI
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private Text timeCounter;
    
        [SerializeField] private Text hpCount;
    
        [SerializeField] private Text lvlCount;
    
        [SerializeField] private Text killCount;
    
        private const string TimeText = "Time: ";

        private const string HealthText = "HP: ";

        private const string LvlText = "LV: ";
    
        private const string KillText = "Kills: ";

        private int _killCount;
    
        private float _timeInLevel;

        private GameObject _player;
    
        private void Start()    //fetches starting values of player
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            hpCount.text = HealthText + _player.GetComponent<Health>().hp;
            lvlCount.text = LvlText + _player.GetComponent<PlayerLeveling>().lvl;
            killCount.text = KillText + _killCount;
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

        public void ChangeKillCount()
        {
            _killCount++;
            killCount.text = KillText + _killCount;
        }
    }
}
