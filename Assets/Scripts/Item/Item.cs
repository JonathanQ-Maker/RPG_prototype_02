
using UnityEngine;

namespace RPG
{
    public class Item : MonoBehaviour
    {
        public Rigidbody2D rb;
        public ItemType itemType;
    }

    public enum ItemType
    { 
        Apple
    }
}
