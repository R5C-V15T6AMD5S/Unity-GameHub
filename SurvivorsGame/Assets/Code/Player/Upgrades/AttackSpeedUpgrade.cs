using UnityEngine;

namespace Code.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewAttackSpeedUpgrade", menuName = "Upgrades/AttackspeedUpgrade")]

    public class AttackSpeedUpgrade : Upgrade
    {
        public AttackSpeedUpgrade()
        {
            upgradeName = "AttackSpeed Upgrade";
            upgradeDescription = "Upgrade your attack speed";
            upgradeLevel = 0;
        }
        public override void ApplyUpgrade()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var playerAttack = player.GetComponent<PlayerAttack>();
            playerAttack.IncreaseAttackSpeed(0.05f);
            upgradeLevel++;
        }
    }
}
