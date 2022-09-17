

using UnityEngine.UI;

namespace RPG
{
    public class EquipmentWindow : InventoryWindow
    {
        public GridLayoutGroup equipmentGridLayout;
        public int equipmentWidth = 1;
        protected override void LoadInventory(Inventory inventory)
        {
            if (inventory == null) return;

            if (gridLayout.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
            {
                gridLayout.constraintCount = inventory.Width - equipmentWidth;
                equipmentGridLayout.constraintCount = equipmentWidth;
            }
            else
            {
                gridLayout.constraintCount = inventory.Height;
                equipmentGridLayout.constraintCount = inventory.Height;
            }

            // for adding slots
            for (int i = inventorySlots.Count; i < inventory.Length; i = inventorySlots.Count)
            {
                if (inventory.GetX(i) >= inventory.Width - equipmentWidth)
                {
                    AddEquipmentSlot();
                }
                else
                {
                    AddInventorySlot();
                }
            }


            // for removing slots
            for (int i = inventorySlots.Count; i > inventory.Length; i = inventorySlots.Count)
            {
                RemoveInventorySlot();
            }

            UpdateContent();
        }

        protected virtual void AddEquipmentSlot()
        {
            ItemSlot slot = Instantiate(prefabSlot, equipmentGridLayout.gameObject.transform);
            slot.InventoryWindow = this;
            slot.SlotIndex = inventorySlots.Count;
            inventorySlots.AddLast(slot);
        }
    }
}
