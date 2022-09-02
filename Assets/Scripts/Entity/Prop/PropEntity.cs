using UnityEngine;

namespace RPG
{
    public class PropEntity : Entity
    {
        // actually interact 
        public virtual void Interact(CharacterEntity interactee)
        {
            
        }

        // used for before-interact animations/effects
        public virtual void OnHover(CharacterEntity interactee)
        { 
            
        }

        // used for before-interact animations/effects
        public virtual void OnUnhover(CharacterEntity interactee)
        {

        }
    }
}
