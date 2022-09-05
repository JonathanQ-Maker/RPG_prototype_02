﻿using UnityEngine;

namespace RPG
{
    /// <summary>
    /// Class used to create the functionality of an item
    /// </summary>
    public abstract class ItemHandler : MonoBehaviour
    {
        public const string TAG = "ItemHandler";
        public Rigidbody2D rb;
        public ItemHandler PrefabRef
        {
            get;
            protected set;
        }

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {
            if (PrefabRef == null)
                throw new System.Exception("prefabRef must be set.");
        }

        public virtual ItemStack GenerateItemStack()
        {
            ItemStack itemStack = new ItemStack(1, PrefabRef);
            return itemStack;
        }

        public virtual void ApplyItemStack(ItemStack itemStack)
        { 
            
        }

        /// <summary>
        /// Called when this item is collected
        /// </summary>
        public virtual void OnCollect()
        {
            Destroy(gameObject);
        }
    }
}