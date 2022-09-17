using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class InventoryWindow : ModalWindow
    {
        public ItemSlot prefabSlot;
        public InventoryItem prefabInventoryItem;
        public GridLayoutGroup gridLayout;
        protected LinkedList<ItemSlot> inventorySlots = new LinkedList<ItemSlot>();
        private Inventory inventory;
        public Inventory Inventory
        {
            get
            {
                return inventory;
            }

            set
            {
                inventory = value;
                LoadInventory(inventory);
            }
        }

        public override bool Active
        {
            get => base.Active;
            set
            {
                base.Active = value;
                if (inventory != null)
                {
                    LoadInventory(Inventory);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
                }
            }
        }

        protected virtual void LoadInventory(Inventory inventory)
        {
            if (inventory == null) return;

            if (gridLayout.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
            {
                gridLayout.constraintCount = inventory.Width;
            }
            else
            {
                gridLayout.constraintCount = inventory.Height;
            }

            // for adding slots
            for (int i = inventorySlots.Count; i < inventory.Length; i = inventorySlots.Count)
            {
                AddInventorySlot();
            }


            // for removing slots
            for (int i = inventorySlots.Count; i > inventory.Length; i = inventorySlots.Count)
            {
                RemoveInventorySlot();
            }

            UpdateContent();
        }

        protected virtual void Update()
        {
            UpdateContent();
        }

        public virtual void UpdateContent()
        {
            if (Inventory == null) return;
            int index = 0;
            foreach (ItemSlot itemSlot in inventorySlots)
            {
                if (Inventory[index] != null)
                {
                    if (itemSlot.inventoryItem == null)
                    {
                        InventoryItem item = Instantiate(prefabInventoryItem, itemSlot.transform);
                        item.Slot = itemSlot;
                        item.ItemIndex = index;
                    }
                    itemSlot.inventoryItem.UpdateItemCount();
                }
                else
                {
                    if (itemSlot.inventoryItem != null)
                    {
                        Destroy(itemSlot.inventoryItem.gameObject);
                    }
                }
                index++;
            }
        }

        protected virtual void AddInventorySlot()
        {
            ItemSlot slot = Instantiate(prefabSlot, gridLayout.gameObject.transform);
            slot.InventoryWindow = this;
            slot.SlotIndex = inventorySlots.Count;
            inventorySlots.AddLast(slot);
        }

        protected virtual void RemoveInventorySlot()
        {
            ItemSlot slot = inventorySlots.Last.Value;
            inventorySlots.RemoveLast();
            Destroy(slot.gameObject);
        }
    }
}
