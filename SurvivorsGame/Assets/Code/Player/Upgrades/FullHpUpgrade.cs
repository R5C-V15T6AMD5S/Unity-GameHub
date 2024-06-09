using UnityEngine;

namespace Code.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewFullHpUpgrade", menuName = "Upgrades/FullHpUpgrade")]
    public class FullHpUpgrade : Upgrade
    {
        public FullHpUpgrade()
        {
            upgradeName = "Full Health";
            upgradeDescription = "Heals you to max health";
            upgradeLevel = 0;
        }
        public override void ApplyUpgrade()
        { 
            var player = GameObject.FindGameObjectWithTag("Player");
            var health = player.GetComponent<Health>();
            health.Heal(health.maxHealth);
        }
    }
}