namespace Code.Player
{
    public abstract class Upgrade : UnityEngine.ScriptableObject
    {
        public string upgradeName;
        public string upgradeDescription;
        public int upgradeLevel;
        public abstract void ApplyUpgrade();
        public void Reset()
        {
            upgradeLevel = 0;
        }
    }
}