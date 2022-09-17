// Uncomment the line below to debug ItemStack memory leak
// #define DEBUG_MEM


using System;
using System.Collections.Generic;

namespace RPG
{
    /// <summary>
    /// Item data container, for when item gameobject represnetation needs to be destroyed.
    /// </summary>
    public class ItemStack : ICloneable<ItemStack>
    {
#if DEBUG_MEM
        private static List<WeakReference> itemStacks = new List<WeakReference>();

        public static int GetAliveCount()
        {
            GC.Collect();
            int count = 0;
            foreach (WeakReference wr in itemStacks)
            {
                if (wr.IsAlive) count++;
            }
            return count;
        }
#endif

        public const int MAX_STACK = 5;
        public PrefabType prefabType;
        public int Count
        {
            get
            {
                return count;
            }

            set
            {
                count = Math.Clamp(value, 0, MAX_STACK); // min count must be 0, it is used to check if is empty
            }
        }
        public string toolTip;

        private int count;

        public ItemStack(int count, PrefabType prefabType, string toolTip)
        {
            this.count = count;
            this.prefabType = prefabType;
            this.toolTip = toolTip;

#if DEBUG_MEM
            itemStacks.Add(new WeakReference(this));
#endif
        }

        public ItemStack(PrefabType prefabType, string toolTip) : this(1, prefabType, toolTip) { }
        public ItemStack(PrefabType prefabType) : this(1, prefabType, "") { }

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
            return other.prefabType == prefabType;
        }

        public override int GetHashCode()
        {
            // possible hash collision even though Equals() returns false
            return base.GetHashCode();
        }

        public ItemStack Clone()
        {
            ItemStack itemStack = new ItemStack(Count, prefabType, toolTip);
            return itemStack;
        }
    }
}
