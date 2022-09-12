﻿using UnityEngine;

namespace RPG
{
    public class CharacterEntity : ControllableEntity, InventoryOwner
    {
        public float dropForce;
        public Rigidbody2D rb;
        public Animator animator;
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

        [SerializeField] // makes editable in unity
        protected float moveSpeed;
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

            UpdateSortingOrder();
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
            inventory = new Inventory(2, 5, this);
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
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - transform.position).normalized;
            Direction = GetRelativeDir(mousePosition);
            ItemHandler handler = Instantiate(itemStack.handlerPrefab, 
                (Vector2)transform.position + direction * 2f, 
                Quaternion.identity);
            handler.ItemStack = itemStack;
            handler.rb.velocity = direction * dropForce;
        }

        public Direction GetRelativeDir(Vector2 position)
        {
            Vector2 delta = position - ((Vector2)transform.position);
            if (delta.x < -0.1f) return Direction.Left;
            if (delta.x > 0.1f) return Direction.Right;
            if (delta.y < -0.1f) return Direction.Down;
            if (delta.y > 0.1f) return Direction.Up;
            return direction;
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
    }

    public enum Direction
    { 
        Down    = 0,
        Left    = 1,
        Up      = 2,
        Right   = 3
    }
}
