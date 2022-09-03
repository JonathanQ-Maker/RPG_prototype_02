using UnityEngine;

namespace RPG
{
    public sealed class InputSystem : Singleton
    {
        // allows easy swapping of control target
        public ControllableEntity controllable;
        public KeyCode interactKey;
        public float interactCoolDown = 0.5f;
        public Vector2 InputAxis 
        {
            get 
            {
                return inputAxis.normalized;
            }
        }


        private Vector2 inputAxis = Vector2.zero;
        private float nextInteractTime;
        

        private void FixedUpdate()
        {
            inputAxis.x = Input.GetAxisRaw("Horizontal");
            inputAxis.y = Input.GetAxisRaw("Vertical");

            controllable.ControlUpdate(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(interactKey) && nextInteractTime < Time.time)
            {
                Debug.Log("Interact");
                controllable.Interact(this);
                nextInteractTime = Time.time + interactCoolDown;
            }
        }
    }
}
