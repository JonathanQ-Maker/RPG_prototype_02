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
                yield return new WaitForSeconds(tickPeriod);
                User.Heal(regenPerTick, null); // heal after wait to avoid effect cooldown abuse by swapping into slot
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
