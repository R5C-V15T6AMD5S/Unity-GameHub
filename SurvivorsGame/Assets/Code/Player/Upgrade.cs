namespace Code.Player
{
    public abstract class Upgrade : UnityEngine.ScriptableObject
    {
        public string upgradeName;
        public string upgradeDescription;
        public int upgradeLevel;
        public abstract void ApplyUpgrade(); // Apply the upgrade to the player
        public void Reset() // Reset the upgrade level
        {
            upgradeLevel = 0;
        }
    }
}