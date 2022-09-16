using UnityEngine;
namespace RPG
{
    public interface IInventoryOwner
    {
        public abstract void DropItem(ItemStack itemStack);

        // ENSURE BI-DIRECTIONAL. SEE BELOW
        public Inventory Inventory { get; set; }
    }


    // BI-DIRECTIONAL EXAMPLE (READY TO COPY-PASTE):

    // //  PASTE IN INVENTORY OWNER
    //private Inventory inventory;
    //public Inventory Inventory
    //{
    //    get
    //    {
    //        return inventory;
    //    }


    //    set
    //    {
    //        if (inventory != null && inventory != value)
    //        {
    //            Inventory temp = inventory;
    //            inventory = null;
    //            temp.Owner = null;
    //        }

    //        inventory = value;
    //        if (inventory != null && inventory.Owner != this)
    //        {
    //            inventory.Owner = this;
    //        }
    //    }
    //}
}
