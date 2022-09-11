
using UnityEngine;

namespace RPG
{
    public abstract class Entity : MonoBehaviour
    {
        public const int ORDER_MULTIPLIER = 10;
        public int baseOrder;
        public SpriteRenderer spriteRenderer;

        [SerializeField]
        private int maxHealth = 1;

        [SerializeField]
        private int health = 1;

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

        /// <summary>
        /// Mostly use this for internal animation-less functionalities
        /// See also: Hurt() Heal()
        /// </summary>
        public virtual int Health
        {
            set
            {
                int oldHealth = health;
                health = Mathf.Clamp(value, 0, MaxHealth);
                OnHealthChange(oldHealth);
            }

            get
            {
                return health;
            }
        }

        protected virtual void Start()
        {
            Health = MaxHealth;
            UpdateSortingOrder();
        }

        protected virtual void UpdateSortingOrder()
        {
            spriteRenderer.sortingOrder = baseOrder - (int)(transform.position.y * ORDER_MULTIPLIER);
        }

        protected virtual void OnHealthChange(int oldHealth)
        { 
            
        }

        /// <summary>
        /// Health decrease with animation
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="attacker"></param>
        public virtual void Hurt(int damage, Entity attacker)
        {
            Health -= damage;
        }

        /// <summary>
        /// Health increase with animation
        /// </summary>
        /// <param name="healing"></param>
        /// <param name="healer"></param>
        public virtual void Heal(int healing, Entity healer)
        {
            Health += healing;
        }
    }
}
