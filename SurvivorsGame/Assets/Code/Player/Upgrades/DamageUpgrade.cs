using UnityEngine;

namespace Code.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewDamageUpgrade", menuName = "Upgrades/DamageUpgrade")]
    public class DamageUpgrade : Upgrade
    {
        public DamageUpgrade()
        {
            upgradeName = "Damage Upgrade";
            upgradeDescription = "Upgrade your damage";
            upgradeLevel = 0;
        }
        public override void ApplyUpgrade()
        {
            var attackArea = GameObject.FindGameObjectWithTag("Player").GetComponent<AttackArea>();
            attackArea.IncreaseDamage(5);
            upgradeLevel++;
            Debug.Log("Applying upgrade: " + upgradeName + " level " + upgradeLevel);
        }
    }
}