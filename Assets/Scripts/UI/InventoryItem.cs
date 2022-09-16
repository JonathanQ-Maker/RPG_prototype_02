using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace RPG
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        public TextMeshProUGUI footnote;

        private ItemSlot slot;
        public ItemSlot Slot
        { 
            get
            {
                return slot;
            }

            set
            {
                if (value != null)
                {
                    value.inventoryItem = this;
                    transform.SetParent(value.transform);
                    if (ItemIndex != -1)
                    {
                        ItemStack itemStack = slot.InventoryWindow.Inventory.Pop(ItemIndex);
                        value.InventoryWindow.Inventory[value.SlotIndex] = itemStack;
                        slot = value;
                        ItemIndex = value.SlotIndex;
                    }
                }
                else
                {
                    if (value.InventoryWindow.Inventory.Owner != null)
                    {
                        value.InventoryWindow.Inventory.Owner.DropItem(value.InventoryWindow.Inventory.Pop(ItemIndex));
                    }
                    Destroy(gameObject);
                    return;
                }
                slot = value;
                ResetPosition();
            }
        }

        private int itemIndex = -1;
        public int ItemIndex
        {
            get 
            {
                return itemIndex;
            }

            set
            {
                itemIndex = value;
                image.sprite = slot.InventoryWindow.Inventory[ItemIndex].handlerPrefab.GetComponent<SpriteRenderer>().sprite;
                UpdateItemCount();
            }
        }

        public ItemStack ItemStack
        {
            get
            { 
                return Slot.InventoryWindow.Inventory[ItemIndex];
            }
        }

        public void UpdateItemCount()
        {
            footnote.text = string.Format("{0}", ItemStack.Count);
        }

        public void ResetPosition()
        {
            transform.SetParent(null);
            transform.SetParent(slot.transform);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            (transform as RectTransform).anchoredPosition += eventData.delta / slot.InventoryWindow.Canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(Slot.InventoryWindow.WindowRect, Input.mousePosition))
            {
                if (Slot.InventoryWindow.Inventory.Owner != null)
                {
                    Slot.InventoryWindow.Inventory.Owner.DropItem(ItemStack);
                }
                Slot.InventoryWindow.Inventory[ItemIndex] = null;
                Destroy(gameObject);
            }
            else
            {
                ResetPosition();
            }
            canvasGroup.blocksRaycasts = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DisplaySystem.Instance.toolTipWindow.Active = true;
            DisplaySystem.Instance.toolTipWindow.LoadItemTip(ItemStack);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DisplaySystem.Instance.toolTipWindow.Active = false;
        }

        private void OnDestroy()
        {
            DisplaySystem.Instance.toolTipWindow.Active = false;
        }

        private void OnDisable()
        {
            DisplaySystem.Instance.toolTipWindow.Active = false;
        }
    }
}
