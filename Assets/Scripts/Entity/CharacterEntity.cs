using UnityEngine;

namespace RPG
{
    public class CharacterEntity : ControllableEntity
    {
        public Rigidbody2D rb;
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Inventory inventory;
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

        [SerializeField] // makes editable in unity
        private float moveSpeed;
        private PropEntity targetPropEntity;
        private Direction direction;

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
            inventory = new Inventory(1, 1);
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
                // TODO: try collect item
                ItemHandler itemHandler = collision2D.gameObject.GetComponent<ItemHandler>();
                if (itemHandler != null)
                {
                    if (inventory.Add(itemHandler.GenerateItemStack()))
                    {
                        itemHandler.OnCollect();
                    }
                    Debug.Log(inventory);
                }
            }
        }

        public override void Interact(InputSystem inputSystem)
        {
            if (TargetPropEntity != null)
                TargetPropEntity.Interact(this);
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
