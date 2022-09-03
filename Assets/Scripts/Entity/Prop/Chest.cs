using UnityEngine;

namespace RPG
{
    public class Chest : PropEntity
    {
        public float hoverScale = 1.1f;
        public Animator animator;

        public override void OnHover(CharacterEntity interactee)
        {
            transform.localScale *= hoverScale;
        }

        public override void OnUnhover(CharacterEntity interactee)
        {
            transform.localScale /= hoverScale;
        }

        public override void Interact(CharacterEntity interactee)
        {
            animator.SetBool("Open", !animator.GetBool("Open"));
        }
    }
}
