using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG
{
    public class ItemSlot : MonoBehaviour, IDropHandler
    {
        private InventoryWindow inventoryWindow;
        public InventoryWindow InventoryWindow
        {
            get
            {
                return inventoryWindow;
            }

            set
            {
                if (inventoryWindow == null)
                {
                    inventoryWindow = value;
                    return;
                }
                Debug.LogWarning("Cannot re-assign inventoryWindow");
            }
        }

        private int slotIndex = -1;
        public int SlotIndex
        {
            get
            {
                return slotIndex;
            }

            set
            {
                if (slotIndex == -1)
                {
                    slotIndex = value;
                    return;
                }
                Debug.LogWarning("Cannot re-assign slotIndex");
            }
        }

        public InventoryItem inventoryItem;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                if (inventoryItem != null)
                {
                    if (this.inventoryItem == null)
                    {
                        inventoryItem.Slot = this;
                    }
                }
            }
        }
    }
}
