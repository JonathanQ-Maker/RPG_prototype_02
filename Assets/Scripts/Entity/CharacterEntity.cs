using UnityEngine;

namespace RPG
{
    public class CharacterEntity : ControllableEntity, InventoryOwner
    {
        public float dropForce;
        public Rigidbody2D rb;
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public float MoveSpeed
        {
            set
            {
                moveSpeed = Mathf.Max(value, 0);
            }

            get
            {
                return moveSpeed;
            }
        }

        public Inventory Inventory
        {
            get
            {
                return inventory;
            }


            set
            {
                if (inventory != null && inventory != value)
                {
                    Inventory temp = inventory;
                    inventory = null;
                    temp.Owner = null;
                }

                inventory = value;
                if (inventory != null && inventory.Owner != this)
                {
                    inventory.Owner = this;
                }
            }
        }

        [SerializeField] // makes editable in unity
        private float moveSpeed;
        private PropEntity targetPropEntity;
        private Direction direction;
        private Inventory inventory;

        protected PropEntity TargetPropEntity
        {
            set 
            {
                if (targetPropEntity != null && targetPropEntity != value)
                {
                    targetPropEntity.OnUnhover(this);   // used for before-interact animations/effects
                }

                targetPropEntity = value;
                if (value != null)
                {
                    targetPropEntity.OnHover(this);     // used for before-interact animations/effects
                }
            }

            get
            {
                return targetPropEntity;
            }
        }

        public override void ControlUpdate(InputSystem inputSystem)
        {
            // move character
            if (rb != null)
            rb.velocity = inputSystem.InputAxis * MoveSpeed;

            // update animator params
            if (animator != null)
            {
                direction = GetInputDirection(inputSystem);

                animator.SetFloat("Speed", rb.velocity.sqrMagnitude); // squared magnitude is cheaper to calc
                animator.SetInteger("Direction", (int)direction);
            }

            // update sortingOrder
            spriteRenderer.sortingOrder = baseOrder - (int)(transform.position.y * ORDER_MULTIPLIER);
        }

        protected virtual Direction GetInputDirection(InputSystem inputSystem)
        {
            // when moving up left, show left as usually left sprites are more expressive
            if (rb.velocity.x < -1f)    return Direction.Left;
            if (rb.velocity.x > 1f)     return Direction.Right;
            if (rb.velocity.y < -1f)    return Direction.Down;
            if (rb.velocity.y > 1f)     return Direction.Up;
            return direction;
        }

        protected virtual void Awake()
        {
            inventory = new Inventory(2, 2, this);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider2D) 
        {
            PropEntity entity = collider2D.GetComponent<PropEntity>();  // try get PropEntity type component
            if (entity != null)             // did we find the component?
            {
                TargetPropEntity = entity;                              // utilize getters/setters defined in TargetPropEntity
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collider2D)
        {
            PropEntity entity = collider2D.GetComponent<PropEntity>();
            if (entity != null)
            {
                if (TargetPropEntity != null)
                {
                    if (entity == TargetPropEntity)
                        TargetPropEntity = null;
                }
                else TargetPropEntity = null;
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.gameObject.tag == ItemHandler.TAG)
            {
                // try collect item
                ItemHandler itemHandler = collision2D.gameObject.GetComponent<ItemHandler>();
                if (itemHandler != null)
                {
                    if (inventory.Add(itemHandler.ItemStack))
                    {
                        itemHandler.OnCollect();
                    }
                }
            }
        }

        public override void Interact(InputSystem inputSystem)
        {
            if (TargetPropEntity != null)
                TargetPropEntity.Interact(this);
        }

        /// <summary>
        /// Drops item in context of this CharacterEntity, DOES NOT REMOVE FROM THIS INVENTORY
        /// make sure to set corresponding ItemStack slot in inventory to null
        /// </summary>
        /// <param name="itemStack"></param>
        public void DropItem(ItemStack itemStack)
        {
            ItemHandler handler = Instantiate(itemStack.handlerPrefab, 
                transform.position + new Vector3(0, 1f, 0), 
                Quaternion.identity);
            handler.ItemStack = itemStack;
            handler.rb.velocity = Random.insideUnitCircle * dropForce;
        }
    }

    public enum Direction
    { 
        Down    = 0,
        Left    = 1,
        Up      = 2,
        Right   = 3
    }
}
