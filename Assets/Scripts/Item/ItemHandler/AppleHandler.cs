using UnityEngine;

namespace RPG
{
    public class AppleHandler : ItemHandler
    {
        protected override void Awake()
        {
            prefabType = PrefabType.Apple;
        }
    }
}
