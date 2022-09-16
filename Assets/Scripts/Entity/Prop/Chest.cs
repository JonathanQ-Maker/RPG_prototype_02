using UnityEngine;

namespace RPG
{
    public class Chest : PropEntity, IInventoryOwner
    {
        public float hoverScale = 1.1f;
        public Animator animator;

        public Inventory Inventory
        {
            get
            {
                return inventory;
            }


            set
            {
                if (inventory != null && inventory != value)
                {
                    Inventory temp = inventory;
                    inventory = null;
                    temp.Owner = null;
                }

                inventory = value;
                if (inventory != null && inventory.Owner != this)
                {
                    inventory.Owner = this;
                }
            }
        }

        private Inventory inventory;

        protected override void Start()
        {
            base.Start();
            inventory = new Inventory(2,2, this);
        }

        public override void OnHover(CharacterEntity interactee)
        {
            transform.localScale *= hoverScale;
        }

        public override void OnUnhover(CharacterEntity interactee)
        {
            transform.localScale /= hoverScale;
            animator.SetBool("Open", false);
            DisplaySystem.Instance.propInventoryWindow.Active = false;
        }

        public override void Interact(CharacterEntity interactee)
        {
            animator.SetBool("Open", !animator.GetBool("Open"));
            DisplaySystem.Instance.propInventoryWindow.Inventory = Inventory;
            DisplaySystem.Instance.propInventoryWindow.Header = "Chest";
            DisplaySystem.Instance.propInventoryWindow.Active = true;
        }

        public void DropItem(ItemStack itemStack)
        {
            Vector2 direction = MathUtil.RandomPointUnitCircle();
            ItemHandler handler = Instantiate(itemStack.handlerPrefab,
                (Vector2)transform.position + direction * 2f,
                Quaternion.identity);
            handler.ItemStack = itemStack;
            handler.rb.velocity = direction * Random.value * 5f;
        }
    }
}
