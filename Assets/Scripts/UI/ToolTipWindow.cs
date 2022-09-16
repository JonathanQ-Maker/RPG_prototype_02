using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPG
{
    public class ToolTipWindow : MonoBehaviour
    {
        public TextMeshProUGUI toolTip;
        public Canvas canvas;
        public bool Active
        {
            get
            {
                return gameObject.activeSelf;
            }

            set
            {
                // Cursor.visible = !value;
                if (!gameObject.activeSelf && value)
                {
                    transform.SetAsLastSibling();
                    gameObject.SetActive(value);
                }
                else
                {
                    gameObject.SetActive(value);
                }
            }
        }

        public void LoadItemTip(ItemStack itemStack)
        {
            toolTip.text = itemStack.GetToolTip();
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        private void SetPosition(Vector2 position)
        {
            RectTransform rectTransform = transform as RectTransform;
            RectTransform canvasTransform = canvas.transform as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, position, null, out Vector2 localpoint);
            Vector2 normalizedPoint = Rect.PointToNormalized(canvasTransform.rect, localpoint);
            rectTransform.anchorMin = normalizedPoint;
            rectTransform.anchorMax = normalizedPoint;
        }

        protected void Update()
        {
            SetPosition(Input.mousePosition);
        }
    }
}
