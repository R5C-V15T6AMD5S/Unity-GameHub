using UnityEngine;

namespace Code.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewHealthUpgrade", menuName = "Upgrades/HealthUpgrade")]
    public class HpUpgrade : Upgrade
    {
        public HpUpgrade()
        {
            upgradeName = "Health Upgrade";
            upgradeDescription = "Upgrade your health";
            upgradeLevel = 0;
        }
        public override void ApplyUpgrade()
        {
            var health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            health.SetHealth(health.maxHealth + 5, health.hp + 5);
            upgradeLevel++;
            Debug.Log("Applying upgrade: " + upgradeName + " level " + upgradeLevel);
        }
    }
}