using UnityEngine;
using System;

namespace RPG
{
    /// <summary>
    /// Item data container, for when item gameobject represnetation needs to be destroyed.
    /// </summary>
    public class ItemStack
    {
        public const int MAX_STACK = 5;
        public ItemHandler handlerPrefab;
        public int Count
        {
            get
            {
                return count;
            }

            set
            {
                count = Math.Clamp(value, 0, MAX_STACK);
            }
        }

        private int count;

        public ItemStack(int count, ItemHandler handlerPrefab)
        {
            this.count = count;
            this.handlerPrefab = handlerPrefab;
        }

        /// <summary>
        /// Tries to add contents from other ItemStack to this
        /// </summary>
        /// <param name="other">other ItemStack</param>
        /// <returns>is other ItemStack empty</returns>
        public bool Add(ItemStack other)
        {
            if (other == this)
            {
                int sum = other.Count + Count;
                int leftOver = Math.Max(sum - MAX_STACK, 0);
                Count = sum - leftOver;
                other.Count = leftOver;
            }
            return other.Count <= 0;
        }

        /// <summary>
        /// Only checks if the Item handler is the same
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(ItemStack a, ItemStack b)
        {
            if (a is null)
                return b is null;

            return a.Equals(b);
        }
        public static bool operator !=(ItemStack a, ItemStack b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ItemStack other = (ItemStack)obj;
            return other.handlerPrefab == handlerPrefab;
        }

        public override int GetHashCode()
        {
            // possible hash collision even though Equals() returns false
            return base.GetHashCode();
        }
    }
}
