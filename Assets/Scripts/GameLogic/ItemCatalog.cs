using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public sealed class ItemCatalog  : MonoBehaviour
    {
        public static ItemCatalog  Instance { get; private set; }

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

        private void Awake()
        {
            CheckInstance(); // Always first
        }

        //########################################################
        //# Catalog
        //#######################################################

        public readonly ItemStack Apple = new ItemStack(1, PrefabType.Apple, "Apple\n\nMaterial", new AppleHandler());
        public readonly ItemStack RingOfVitality = new ItemStack(1, PrefabType.RingOfVitality, "Ring of Vitality\n\nEquipment", new RingOfVitalityHandler());
        public readonly ItemStack ShortSword = new ItemStack(1, PrefabType.ShortSword, "Short Sword\n\n1 Attack Damage", new ShortSwordHandler());
    }
}
