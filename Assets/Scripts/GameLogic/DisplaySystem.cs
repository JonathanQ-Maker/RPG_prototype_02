using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG
{
    public sealed class DisplaySystem : MonoBehaviour
    {
        public static DisplaySystem Instance { get; private set; }

        // Unity Singelton Modeled from https://gamedevbeginner.com/singletons-in-unity-the-right-way/#:~:text=Generally%20speaking%2C%20a%20singleton%20in,or%20to%20other%20game%20systems.
        private void CheckInstance()
        { 
            // If there is an instance, and it's not me, delete myself.
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }




        //########################################################
        //# Game Logic
        //########################################################

        public ToolTipWindow toolTipWindow;
        public InventoryWindow propInventoryWindow;
        public EquipmentWindow inventoryWindow;
        public Slider healthSider;
        public TMP_Text healthText;
        public int DisplayHealth
        {
            set
            {
                healthSider.value = value;
                UpdateHealthText();
            }

            get
            {
                return (int)healthSider.value;
            }
        }
        public int MaxDisplayHealth
        {
            set
            {
                healthSider.maxValue = value;
                UpdateHealthText();
            }

            get
            {
                return (int)healthSider.maxValue;
            }
        }

        private void Awake()
        {
            CheckInstance(); // always first

            CheckHealthSlider();
        }

        private void CheckHealthSlider()
        {
            if (!healthSider.wholeNumbers)
            {
                Debug.LogWarning("DisplaySystem: healthSider value is not set to whole numbers.");
            }
        }

        public void UpdateHealthText()
        {
            healthText.text = string.Format("{0}/{1}", DisplayHealth, MaxDisplayHealth);
        }

        /// <summary>
        /// toggles character inventory, equipment UI window states.
        /// </summary>
        /// <returns></returns>
        public bool ToggleInventory()
        {
            inventoryWindow.Active = !inventoryWindow.Active;
            return inventoryWindow.Active;
        }

        /// <summary>
        /// toggles prop inventory, equipment UI window states.
        /// </summary>
        /// <returns></returns>
        public bool TogglePropInventory()
        {
            propInventoryWindow.Active = !propInventoryWindow.Active;
            return propInventoryWindow.Active;
        }
    }
}
