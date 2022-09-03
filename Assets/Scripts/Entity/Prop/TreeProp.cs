using UnityEngine;

namespace RPG
{
    public class TreeProp : PropEntity
    {
        public float hoverScale = 1.1f;
        public Sprite stump;

        public override void OnHover(CharacterEntity interactee)
        {
            // this cannot be one line, it does not scale correcly
            // when you scale up, scale down and try scale up again.
            // I have no idea why.
            transform.localScale *= hoverScale; 
        }

        public override void OnUnhover(CharacterEntity interactee)
        {
            transform.localScale /= hoverScale;
        }

        public override void Interact(CharacterEntity interactee)
        {
            spriteRenderer.sprite = stump;
        }
    }
}
