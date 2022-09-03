
using UnityEngine;

namespace RPG
{
    public abstract class Entity : MonoBehaviour
    {
        public const int ORDER_MULTIPLIER = 10;
        public int baseOrder;

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
