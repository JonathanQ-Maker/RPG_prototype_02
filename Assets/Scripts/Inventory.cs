

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
        public InventoryOwner Owner
        {
            get
            {
                return owner;
            }


            set
            {
                if (owner != null && owner != value)
                {
                    InventoryOwner temp = owner;
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
        private InventoryOwner owner;

        public Inventory(int width, int height, InventoryOwner owner)
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
