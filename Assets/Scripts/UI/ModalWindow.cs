using UnityEngine;
using TMPro;

namespace RPG
{
    public class ModalWindow : MonoBehaviour
    {
        public TextMeshProUGUI HeaderText;
        public RectTransform WindowRect;
        public Canvas Canvas { get; private set; }
        public virtual string Header
        {
            get
            {
                if (HeaderText != null)
                    return HeaderText.text;
                else
                    return null;
            }

            set
            {
                if (HeaderText != null)
                    HeaderText.text = value;
                else
                    Debug.LogWarning(name + " does not have a header text object");
            }
        }

        public virtual bool Active
        {
            get
            {
                return gameObject.activeSelf;
            }

            set
            {
                gameObject.SetActive(value);
            }
        }

        protected virtual void Awake()
        {
            Canvas = GetComponentInParent<Canvas>();
        }
    }
}
