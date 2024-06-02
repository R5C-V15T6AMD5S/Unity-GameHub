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
            var child = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);
            var attackArea = child.GetComponent<AttackArea>();
            Debug.Log(attackArea);
            attackArea.IncreaseDamage(1);
            upgradeLevel++;
            Debug.Log("Applying upgrade: " + upgradeName + " level " + upgradeLevel);
        }
    }
}