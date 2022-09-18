using UnityEngine;

namespace RPG
{
    /// <summary>
    /// Class used to create the functionality of an item
    /// </summary>
    public abstract class ItemHandler
    {
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
    }
}
