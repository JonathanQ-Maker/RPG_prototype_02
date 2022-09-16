

using System;
using System.Text;

namespace RPG
{
    public class Inventory
    {

        public int Width
        {
            get 
            {
                return width;
            }

            private set
            {
                width = Math.Max(value,1);
            }
        }
        public int Height
        {
            get
            {
                return height;
            }

            private set
            {
                height = Math.Max(value, 1);
            }
        }
        public int Length
        {
            get 
            {
                return slots.Length;
            }
        }
        public IInventoryOwner Owner
        {
            get
            {
                return owner;
            }


            set
            {
                if (owner != null && owner != value)
                {
                    IInventoryOwner temp = owner;
                    owner = value;
                    temp.Inventory = null;
                }

                owner = value;
                if (owner != null && owner.Inventory != this)
                {
                    owner.Inventory = this;
                }
            }
        }
        public ItemStack this[int index]
        {
            get
            {
                return slots[index];
            }

            set
            {
                if (index < 0 || index >= slots.Length)
                    throw new IndexOutOfRangeException("Slot position out of bounds.");
                onInventoryChange?.Invoke(this, value, index % Width, index / Width);
                slots[index] = value;
            }
        }
        public ItemStack this[int x, int y]
        {
            get
            {
                return slots[y * width + x];
            }

            set
            {
                this[y * width + x] = value;
            }
        }
        public OnInventoryChange onInventoryChange;


        private int width, height;
        private ItemStack[] slots;
        private IInventoryOwner owner;

        public Inventory(int width, int height, IInventoryOwner owner)
        {
            Width = width;
            Height = height;
            slots = new ItemStack[Width * Height];
            Owner = owner;
        }

        public ItemStack Pop(int index)
        {
            ItemStack itemStack = this[index];
            this[index] = null;
            return itemStack;
        }

        public ItemStack Pop(int x, int y)
        {
            return Pop(y * Width + x);
        }

        /// <summary>
        /// <para>Adds itemStack to inventory</para>
        /// <para>time complexity O(n)</para>
        /// </summary>
        /// <param name="itemStack">itemStack to add</param>
        /// <returns>did itemStack fit</returns>
        public bool Add(ItemStack itemStack)
        {
            ItemStack item = FindNotFull(itemStack);
            if (item != null)
            {
                if (item.Add(itemStack))
                {
                    return true;
                }
            }
            for (int i = 0; i < Length; i++)
            {
                if (slots[i] == null)
                {
                    slots[i] = itemStack;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// <para>scans through inventory for itemStack</para>
        /// <para>time complexity O(n)</para>
        /// </summary>
        /// <param name="itemStack">target item</param>
        /// <returns>equal ItemStack</returns>
        public ItemStack Find(ItemStack itemStack)
        {
            foreach (ItemStack item in slots)
            {
                if (item == itemStack) return item;
            }
            return null;
        }

        /// <summary>
        /// <para>scans through inventory for itemStack with Count < MAX_STACK</para>
        /// <para>time complexity O(n)</para>
        /// </summary>
        /// <param name="itemStack">target item</param>
        /// <returns>equal ItemStack</returns>
        public ItemStack FindNotFull(ItemStack itemStack)
        {
            foreach (ItemStack item in slots)
            {
                if (item == itemStack && item.Count < ItemStack.MAX_STACK) return item;
            }
            return null;
        }

        public int GetX(int index)
        {
            return index % Width;
        }

        public int GetY(int index)
        {
            return index / Width;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < Width; x++)
            {
                sb.Append("---");
            }
            sb.Append("\n");

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    sb.AppendFormat("|{0}|", this[x, y] != null ? this[x, y].Count : -1);
                }
                sb.Append("\n");
                for (int x = 0; x < Width; x++)
                {
                    sb.Append("---");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }

    // Delegate definitions

    /// <summary>
    /// Called before inventory is actually changed
    /// </summary>
    /// <param name="inventory">changing inventory</param>
    /// <param name="newStack">ItemStack to change into</param>
    /// <param name="targetX">slot x index</param>
    /// <param name="targetY">slot y index</param>
    /// <returns>should the change go through</returns>
    public delegate void OnInventoryChange(Inventory inventory, ItemStack newStack, int targetX, int targetY);
}
