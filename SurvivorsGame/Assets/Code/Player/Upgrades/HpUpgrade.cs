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
            health.SetHealth(health.maxHealth + 10, health.hp + 10);
            upgradeLevel++;
            Debug.Log("Applying upgrade: " + upgradeName + " level " + upgradeLevel);
        }
    }
}