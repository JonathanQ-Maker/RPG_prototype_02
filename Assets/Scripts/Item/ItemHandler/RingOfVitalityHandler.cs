using System.Collections;
using UnityEngine;

namespace RPG
{
    public class RingOfVitalityHandler : EquipmentHandler
    {
        public int regenPerTick = 1;
        public float tickPeriod = 1f;

        private IEnumerator regenEnum;

        private IEnumerator Regenerate()
        {
            while (User != null)
            {
                User.Heal(regenPerTick, null);
                yield return new WaitForSeconds(tickPeriod);
            }
        }

        public override void OnEquip(CreatureEntity user)
        {
            base.OnEquip(user);
            regenEnum = Regenerate();
            User.StartCoroutine(regenEnum);
        }

        public override void OnUnequip()
        {
            User.StopCoroutine(regenEnum);
            base.OnUnequip();
        }
    }
}
