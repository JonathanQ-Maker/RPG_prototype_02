

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
        public ItemStack this[int x, int y]
        {
            get
            {
                return slots[y * width + x];
            }

            set
            {
                if (y * width + x >= slots.Length && y * width + x < 0) 
                    throw new IndexOutOfRangeException("Slot position out of bounds."); 
                if (onInventoryChange != null) 
                    onInventoryChange(this, value, x, y);
                slots[y * width + x] = value;
            }
        }
        public OnInventoryChange onInventoryChange;
        private int width, height;
        private ItemStack[] slots;

        public Inventory(int width, int height)
        {
            Width = width;
            Height = height;
            slots = new ItemStack[Width * Height];
        }

        public ItemStack Pop(int x, int y)
        {
            ItemStack itemStack = this[x, y];
            this[x, y] = null;
            return itemStack;
        }

        public bool Add(ItemStack itemStack)
        {
            for (int i = 0; i < Length; i++)
            {
                if (slots[i] == null)
                {
                    slots[i] = itemStack;
                    return true;
                }
                else
                {
                    if (slots[i].Add(itemStack))
                    {
                        return true;
                    }
                }
            }
            return false;
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
