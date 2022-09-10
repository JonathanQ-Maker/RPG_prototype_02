
using UnityEngine;

namespace RPG
{
    public abstract class Entity : MonoBehaviour
    {
        public const int ORDER_MULTIPLIER = 10;
        public int baseOrder;
        public SpriteRenderer spriteRenderer;

        [SerializeField]
        private int maxHealth;

        [SerializeField]
        private int health;

        public virtual int MaxHealth
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

        public virtual int Health
        {
            set
            {
                int oldHealth = health;
                health = Mathf.Max(value, 0);
                OnHealthChange(oldHealth);
            }

            get
            {
                return health;
            }
        }

        protected virtual void Start()
        {
            UpdateSortingOrder();
        }

        protected virtual void UpdateSortingOrder()
        {
            spriteRenderer.sortingOrder = baseOrder - (int)(transform.position.y * ORDER_MULTIPLIER);
        }

        protected virtual void OnHealthChange(int oldHealth)
        { 
            
        }
    }
}
