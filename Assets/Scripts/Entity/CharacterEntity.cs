using UnityEngine;

namespace RPG
{
    public class CharacterEntity : ControllableEntity
    {
        public Rigidbody2D rb;
        public Animator animator;
        private PropEntity targetPropEntity;

        private Direction direction;
        [SerializeField]
        private float moveSpeed;

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

        protected PropEntity TargetPropEntity
        {
            set 
            {
                if (targetPropEntity != null && targetPropEntity != value)
                {
                    targetPropEntity.OnUnhover(this);   // used for before-interact animations/effects
                }

                if (value != null)
                {
                    targetPropEntity = value;
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

        // Unity built-in function that gets called when colliders overlap
        protected void OnTriggerEnter2D(Collider2D collider2D) 
        {
            PropEntity entity = collider2D.GetComponent<PropEntity>();  // try get PropEntity type component
            if (entity != null)                                         // did we find the component?
            {
                TargetPropEntity = entity;                              // utilize getters/setters defined in TargetPropEntity
            }
        }

        // Unity built-in function that gets called when colliders overlap
        protected void OnTriggerExit2D(Collider2D collider2D)
        {
            PropEntity entity = collider2D.GetComponent<PropEntity>();
            if (entity != null)
            {
                TargetPropEntity = null;
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
