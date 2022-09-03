using UnityEngine;

namespace RPG
{
    public class PropEntity : Entity
    {
        public SpriteRenderer spriteRenderer;
        protected virtual private void Awake()
        {
            spriteRenderer.sortingOrder = baseOrder - (int)(transform.position.y * ORDER_MULTIPLIER);
        }

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
