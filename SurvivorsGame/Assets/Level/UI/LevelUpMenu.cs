using System.Collections.Generic;
using Code.Player;
using TMPro;
using UnityEngine;
namespace Level.UI
{
    public class LevelUpMenu : MonoBehaviour
    {
        [SerializeField]
        private Upgrade[] allUpgrades;
        
        [SerializeField]
        private UpgradeButton[] options;
        
        private bool _isFirstEnable = true;

        private void Start()
        {
            foreach (var upgrade in allUpgrades)
            {
                upgrade.Reset();
            }
            allUpgrades = Resources.LoadAll<Upgrade>("Upgrades");
            var usedUpgrades = new HashSet<Upgrade>();
            foreach (var option in options)
            {
                Upgrade selectedUpgrade;
                do
                {
                    var randomIndex = Random.Range(0, allUpgrades.Length);
                    selectedUpgrade = allUpgrades[randomIndex];
                } while (usedUpgrades.Contains(selectedUpgrade));
                usedUpgrades.Add(selectedUpgrade);
                option.Upgrade = selectedUpgrade;
                var textComponents = option.Button.GetComponentsInChildren<TMP_Text>();
                foreach (var textComponent in textComponents)
                {
                    textComponent.text = textComponent.name switch
                    {
                        "Name" => option.Upgrade.upgradeName,
                        "Lvl" => "Lvl: " + option.Upgrade.upgradeLevel,
                        _ => textComponent.text
                    };
                }
                option.Button.onClick.AddListener(() => {
                    option.Upgrade.ApplyUpgrade();
                    DisableMenu();
                });
            }
        }

        private void OnEnable()
        {
            if (_isFirstEnable)
            {
                _isFirstEnable = false;
                return;
            }
            Debug.Log("Number of options: " + options.Length);
            Debug.Log("Number of upgrades: " + allUpgrades.Length);
            
            var usedUpgrades = new HashSet<Upgrade>();
            foreach (var option in options)
            {
                Upgrade selectedUpgrade;
                do
                {
                    var randomIndex = Random.Range(0, allUpgrades.Length);
                    selectedUpgrade = allUpgrades[randomIndex];
                } while (usedUpgrades.Contains(selectedUpgrade));
                usedUpgrades.Add(selectedUpgrade);
                option.Button.onClick.RemoveAllListeners();
                option.Upgrade = selectedUpgrade;
                var textComponents = option.Button.GetComponentsInChildren<TMP_Text>();
                foreach (var textComponent in textComponents)
                {
                    textComponent.text = textComponent.name switch
                    {
                        "Name" => option.Upgrade.upgradeName,
                        "Lvl" => "Lvl: " + option.Upgrade.upgradeLevel,
                        _ => textComponent.text
                    };
                }
                option.Button.onClick.AddListener(() => {
                    option.Upgrade.ApplyUpgrade();
                    DisableMenu();
                });
            }
        }
        
        private void DisableMenu()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}
