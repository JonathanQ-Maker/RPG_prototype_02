using UnityEngine;

namespace RPG
{
    /// <summary>
    /// Component used to create the functionality of an dropped item
    /// </summary>
    public class DroppedItem : MonoBehaviour
    {
        public const string TAG = "DroppedItem";
        public Rigidbody2D rb;

        public virtual ItemStack ItemStack
        {
            get
            {
                return itemStack;
            }

            set
            {
                itemStack = value;
            }
        }

        protected ItemStack itemStack;

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {
            if (ItemStack == null)
                throw new System.Exception("ItemStack must be set.");
        }

        /// <summary>
        /// Called when this item is collected
        /// </summary>
        public virtual void OnCollect()
        {
            Destroy(gameObject);
        }

        public static DroppedItem Instantiate(ItemStack itemStack, Vector2 position, Quaternion quaternion)
        {
            DroppedItem droppedItem = Instantiate(AssetManager.GetPrefab<DroppedItem>(itemStack.prefabType), position, quaternion);
            droppedItem.itemStack = itemStack;
            return droppedItem;
        }
    }
}
