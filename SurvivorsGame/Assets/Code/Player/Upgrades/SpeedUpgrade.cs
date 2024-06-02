using UnityEngine;

namespace Code.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewSpeedUpgrade", menuName = "Upgrades/SpeedUpgrade")]
    public class SpeedUpgrade : Upgrade
    {
        public SpeedUpgrade()
        {
            upgradeName = "Speed Upgrade";
            upgradeDescription = "Increases player movement speed by 25%";
            upgradeLevel = 0;
        }
        public override void ApplyUpgrade()
        {
            upgradeLevel++;
            var playerController = FindObjectOfType<PlayerController>();
            playerController.IncreaseMovementSpeed(0.1f);
            Debug.Log("Applying upgrade: " + upgradeName + " level " + upgradeLevel);
        }
    }
}
