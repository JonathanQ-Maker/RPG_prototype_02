namespace RPG
{
    public abstract class WeaponHandler : EquipmentHandler
    {
        public int attackDamage = 1;

        public override void OnEquip(CreatureEntity user)
        {
            base.OnEquip(user);
            User.AttackDamage += attackDamage;
        }

        public override void OnUnequip()
        {
            User.AttackDamage -= attackDamage;
            base.OnUnequip();
        }
    }
}
