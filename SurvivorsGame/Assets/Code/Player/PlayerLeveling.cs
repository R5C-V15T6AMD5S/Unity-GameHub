using Level.UI;
using UnityEngine;

namespace Code.Player
{
    public class PlayerLeveling : MonoBehaviour
    {
        private int _expAmount;
    
        private int _levelUpThreshold = 10;
    
        private int _expToLvlUp;
    
        public int lvl = 1;
    
        private GameObject _hud;
    
        [SerializeField]
        public GameObject lvlUpMenuUI;
    
        private void Start()
        {
            _hud = GameObject.FindGameObjectWithTag("HUD");
        }
        public void ExpPickup(int expPickedUp)  //method for picking up exp
        {
            _expAmount += expPickedUp;
            if (_expAmount < _levelUpThreshold) return;
            LevelUp();
        }

        private void LevelUp()  //level up method
        {
            _levelUpThreshold *= 2;
            lvl += 1;
            _hud.GetComponent<HUDManager>().LevelUp();
            lvlUpMenuUI.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("LVL UP, you are now lvl "+ lvl +" , pick an UPGRADE");
        }
    }
}
