using UnityEngine;

namespace RPG
{
    public sealed class InputSystem : Singleton
    {
        // allows easy swapping of control target
        public ControllableEntity controllable;
        private Vector2 inputAxis = Vector2.zero;
        public Vector2 InputAxis 
        {
            get 
            {
                return inputAxis.normalized;
            }
        }

        private void FixedUpdate()
        {
            inputAxis.x = Input.GetAxisRaw("Horizontal");
            inputAxis.y = Input.GetAxisRaw("Vertical");

            controllable.ControlUpdate(this);
        }
    }
}
