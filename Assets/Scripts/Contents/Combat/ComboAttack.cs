using UnityEngine;

public class ComboAttack : CombatBase
{
    public ComboAttack(CombatSlots combatSlots) : base(combatSlots)
    {
    }
    public override void Use()
    {
        // if (equipment == null) return;
        // comboWeaponEntity = equipment.GetWeaponInfo(Defines.CharacterAttackInputType.ComboAttack);
        // if (comboWeaponEntity == null) return;
        // IsAttacking = true;
        //
        // TODO : 어떻게 할건지..
    }
}