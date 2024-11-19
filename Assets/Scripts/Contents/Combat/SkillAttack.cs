using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : CombatBase
{
    private ItemWeaponCombatEntity skillEntity;
    
    public SkillAttack(CombatSlots combatSlots) : base(combatSlots)
    {
        
    }
    
    public override void SetData(ItemEquipableEntity equipEntity)
    {
        base.SetData(equipEntity);
        skillEntity = EquipEntity.weaponCombatEntities[0];
    }

    public override void Execute()
    {
        if (cooltime > 0) return;
        
        CombatData combat = new CombatData(skillEntity);
        cooltime = combat.cooltime;
        combatSlots.ChangeLayerWeight(EquipEntity.combatStyleType);
        combatSlots.Animator.SetFloat(combatSlots.AnimationData.AttackSpeedHash, combatSlots.Condition.CurrentStat.attackSpeed);
        combatSlots.Animator.SetTrigger(combatSlots.AnimationData.SkillHash);
        ApplyAttack(skillEntity.projectilePrefabPath, combat);
    }
}