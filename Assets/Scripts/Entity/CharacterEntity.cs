using UnityEngine;

namespace RPG
{
    public class CharacterEntity : ControllableEntity, IInventoryOwner
    {
        public float dropForce;
        public Transform pivot, point;
        
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
                inventory.onInventoryChange = OnInventoryChange;
                if (inventory != null && inventory.Owner != this)
                {
                    inventory.Owner = this;
                }
            }
        }

        protected virtual Direction Direction
        {
            get
            {
                return direction;
            }

            set
            {
                animator.SetInteger("Direction", (int)value);
                direction = value;
            }
        }

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
                Direction = GetInputDirection(inputSystem);

                animator.SetFloat("Speed", rb.velocity.sqrMagnitude); // squared magnitude is cheaper to calc
            }

            UpdatePivotRotation();
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
            Inventory = new Inventory(3, 5, this);
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
            if (collision2D.gameObject.tag == DroppedItem.TAG)
            {
                // try collect item
                DroppedItem droppedItem = collision2D.gameObject.GetComponent<DroppedItem>();
                if (droppedItem != null)
                {
                    if (inventory.Add(droppedItem.ItemStack))
                    {
                        droppedItem.OnCollect();
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
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - transform.position).normalized;
            Direction = GetRelativeDir(mousePosition);

            DroppedItem handler = DroppedItem.Instantiate(itemStack, 
                (Vector2)transform.position + direction * 2f, 
                Quaternion.identity);
            handler.rb.velocity = direction * dropForce;
        }

        public Direction GetRelativeDir(Vector2 position)
        {
            /*
             *      \  Up / <--- this vector is "vector45"
             *       \   /
             *        \ /
             *  Left   #    Right
             *        / \
             *       /   \
             *      / Down\
            */

            Vector2 delta = (position - (Vector2)transform.position).normalized;
            Vector2 vector45 = new Vector2(0.7071f, 0.7071f);
            float dot = MathUtil.Dot(delta, vector45);

            if (dot > 0 && dot < 1)
            {
                if (Mathf.Asin(delta.y) < Mathf.PI / 4f) return Direction.Right;
                return Direction.Up;
            }
            if (Mathf.Asin(delta.y) > -Mathf.PI / 4f) return Direction.Left;
            return Direction.Down;
        }

        protected virtual void PlayHurtAnim(Direction faceDirection)
        {
            Direction = faceDirection;
            animator.SetTrigger("Hurt");
        }

        public override void Hurt(int damage, Entity attacker)
        {
            base.Hurt(damage, attacker);
            PlayHurtAnim(GetRelativeDir(attacker.transform.position));
        }

        protected virtual void OnAttack()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, 1f);
            foreach (Collider2D collider2D in colliders)
            {
                if (collider2D.TryGetComponent(out Entity entity) && collider2D.gameObject != gameObject)
                {
                    entity.Hurt(1, this);
                }
            }
        }

        protected virtual void UpdatePivotRotation()
        {
            Vector2 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivot.position;
            float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
            pivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        public override void Attack(InputSystem inputSystem)
        {
            Direction = GetRelativeDir(point.position);
            animator.SetTrigger("Attack");
            OnAttack();
        }

        protected virtual void OnInventoryChange(Inventory inventory, ItemStack oldStack, ItemStack newStack, int targetX, int targetY)
        {
            if (oldStack != null && oldStack.itemHandler is EquipmentHandler)
            {
                EquipmentHandler handler = (EquipmentHandler)oldStack.itemHandler;
                if (handler.IsEquipped)
                {
                    handler.OnUnequip();
                }
            }

            if (targetX >= Inventory.Width - DisplaySystem.Instance.inventoryWindow.equipmentWidth)
            {
                if (newStack != null && newStack.itemHandler is EquipmentHandler)
                {
                    EquipmentHandler handler = (EquipmentHandler)newStack.itemHandler;
                    handler.OnEquip(this);
                }
            }
            else
            {
                if (newStack != null && newStack.itemHandler is EquipmentHandler)
                {
                    EquipmentHandler handler = (EquipmentHandler)newStack.itemHandler;
                    if (handler.IsEquipped)
                    {
                        handler.OnUnequip();
                    }
                }
            }
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
