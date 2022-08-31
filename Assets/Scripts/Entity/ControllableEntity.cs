using UnityEngine;

namespace RPG
{
    public class ControllableEntity : Entity
    {
        public virtual void ControlUpdate(InputSystem inputSystem)
        {
            // gets called in fixed update by InputSystem when is the target of control
        }

        public virtual void Attack(InputSystem inputSystem)
        { 
            
        }

        public virtual void Interact(InputSystem inputSystem)
        { 
        
        }
    }
}
