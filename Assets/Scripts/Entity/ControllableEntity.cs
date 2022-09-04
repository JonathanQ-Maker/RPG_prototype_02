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

        protected virtual void Start()
        {
            UpdateStatusDisplay();
        }

        /// <summary>
        /// Gets called in fixed update by GameLogic when this is the target of control.
        /// </summary>
        /// <param name="gameLogic"></param>
        public virtual void ControlUpdate(GameLogic gameLogic)
        {
            
        }

        public virtual void Attack(GameLogic gameLogic)
        { 
            
        }

        /// <summary>
        /// Called when player presses interact control
        /// </summary>
        /// <param name="gameLogic"></param>
        public virtual void Interact(GameLogic gameLogic)
        { 
        
        }

        /// <summary>
        /// Updates value displays on the UI
        /// </summary>
        public virtual void UpdateStatusDisplay()
        {
            if (GameLogic.Instance.controllable == this)
            {
                // MaxHealth update should be first 
                GameLogic.Instance.MaxDisplayHealth = MaxHealth;
                GameLogic.Instance.DisplayHealth = Health;
            }
        }
    }
}
