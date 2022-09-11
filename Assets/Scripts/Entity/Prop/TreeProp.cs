using UnityEngine;

namespace RPG
{
    public class TreeProp : PropEntity
    {
        public float hoverScale = 1.1f;
        public float dropForce = 10f;

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
            Vector2 dropDir = MathUtil.RandomPointUnitCircle();
            ItemHandler itemHandler = Instantiate(ItemCatalog.Instance.APPLE, 
                (Vector2)transform.position + dropDir, 
                Quaternion.identity);
            itemHandler.ItemStack = new ItemStack(1, ItemCatalog.Instance.APPLE);
            itemHandler.rb.velocity = dropDir * dropForce;
        }
    }
}
