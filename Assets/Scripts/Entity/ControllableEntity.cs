using UnityEngine;

namespace RPG
{
    public class ControllableEntity : Entity
    {
        public override int Health
        {
            get => base.Health;
            set
            {
                base.Health = value;
                UpdateStatusDisplay();
            }
        }

        public override int MaxHealth
        {
            get => base.MaxHealth;
            set
            {
                base.MaxHealth = value;
                UpdateStatusDisplay();
            }
        }

        protected override void Start()
        {
            UpdateStatusDisplay();
        }

        /// <summary>
        /// Gets called in fixed update by InputSystem when this is the target of control.
        /// </summary>
        /// <param name="inputSystem"></param>
        public virtual void ControlUpdate(InputSystem inputSystem)
        {
            
        }

        public virtual void Attack(InputSystem inputSystem)
        { 
            
        }

        /// <summary>
        /// Called when player presses interact control
        /// </summary>
        /// <param name="inputSystem"></param>
        public virtual void Interact(InputSystem inputSystem)
        { 
        
        }

        /// <summary>
        /// Updates value displays on the UI
        /// </summary>
        public virtual void UpdateStatusDisplay()
        {
            if (InputSystem.Instance.Controllable == this)
            {
                // MaxHealth update should be first 
                DisplaySystem.Instance.MaxDisplayHealth = MaxHealth;
                DisplaySystem.Instance.DisplayHealth = Health;
            }
        }
    }
}
