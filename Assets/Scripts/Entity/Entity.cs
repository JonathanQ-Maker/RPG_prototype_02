
using UnityEngine;

namespace RPG
{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth;

        [SerializeField]
        private int health;

        public int MaxHealth
        {
            set
            {
                maxHealth = Mathf.Max(value, 1);
            }

            get
            {
                return maxHealth;
            }
        }

        public int Health
        {
            set
            {
                health = Mathf.Max(value, 0);
            }

            get
            {
                return health;
            }
        }
    }
}
