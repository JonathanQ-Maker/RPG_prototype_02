using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class EquipmentHandler : ItemHandler
    {
        public CreatureEntity User{ get; private set; }
        public virtual bool IsEquipped 
        {
            get {
                return User != null;
            }
        }


        public virtual void OnEquip(CreatureEntity user)
        {
            User = user;
        }

        public virtual void OnUnequip()
        {
            User = null;
        }

        public virtual void EquipmentUpdate()
        { 
            
        }
    }
}
