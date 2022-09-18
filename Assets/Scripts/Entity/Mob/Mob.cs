
using System.Collections;
using UnityEngine;

namespace RPG
{
    public class Mob : CreatureEntity
    {
        public bool IsDead
        {
            get
            {
                return Health <= 0;
            }
        }
    }
}
