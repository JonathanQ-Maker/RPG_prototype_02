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
            inventory[0] = ItemCatalog.Instance.RingOfVitality.Clone();
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
            if (animator.GetBool("Open"))
            {
                DisplaySystem.Instance.propInventoryWindow.Inventory = Inventory;
                DisplaySystem.Instance.propInventoryWindow.Header = "Chest";
                DisplaySystem.Instance.propInventoryWindow.Active = true;
            }
            else
            {
                DisplaySystem.Instance.propInventoryWindow.Inventory = null;
                DisplaySystem.Instance.propInventoryWindow.Active = false;
            }
        }

        public void DropItem(ItemStack itemStack)
        {
            Vector2 direction = MathUtil.RandomPointUnitCircle();
            DroppedItem droppedItem = DroppedItem.Instantiate(itemStack,
                (Vector2)transform.position + direction * 2f,
                Quaternion.identity);
            droppedItem.rb.velocity = direction * Random.value * 5f;
        }
    }
}
