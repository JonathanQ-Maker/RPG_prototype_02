using UnityEngine;

namespace RPG
{
    /// <summary>
    /// Class used to create the functionality of an item
    /// </summary>
    public abstract class ItemHandler : MonoBehaviour
    {
        public const string TAG = "ItemHandler";
        public Rigidbody2D rb;
        public PrefabType prefabType
        {
            get;
            protected set;
        }

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
            if (prefabType == PrefabType.Null)
                throw new System.Exception("prefabType must be set.");
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

        public static ItemHandler Instantiate(ItemStack itemStack, Vector2 position, Quaternion quaternion)
        {
            ItemHandler handler = Instantiate(AssetManager.GetPrefab<ItemHandler>(itemStack.prefabType), position, quaternion);
            handler.itemStack = itemStack;
            return handler;
        }
    }
}
