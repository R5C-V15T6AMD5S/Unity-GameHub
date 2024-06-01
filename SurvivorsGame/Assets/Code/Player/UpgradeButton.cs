using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Code.Player
{
    public class UpgradeButton : MonoBehaviour
    {
        public Button Button { get; private set; }
        public Upgrade Upgrade { get; set; }
        
        private TextMeshProUGUI ButtonText { get; set; }

        private void Awake()
        {
            Button = GetComponent<Button>();
            ButtonText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}