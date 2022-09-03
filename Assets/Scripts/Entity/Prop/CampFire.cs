using UnityEngine;

namespace RPG
{
    public class CampFire : PropEntity
    {

        public float hoverScale = 1.1f;

        public override void OnHover(CharacterEntity interactee)
        {
            transform.localScale *= hoverScale;
        }

        public override void OnUnhover(CharacterEntity interactee)
        {
            transform.localScale /= hoverScale;
        }
    }
}
