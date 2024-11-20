using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : CombatBase
{
    private ItemWeaponCombatEntity skillEntity;
    
    private float freezeTime;
    
    
    public SkillAttack(CombatSlots combatSlots) : base(combatSlots)
    {
        
    }
    
    public override void Update()
    {
        base.Update();
        if (combatSlots.IsSkillCasting)
        {
            freezeTime -= Time.deltaTime;
            if (freezeTime <= 0)
            {
                combatSlots.IsSkillCasting = false;
            }
        }
    }
    
    public override void SetData(ItemEquipableEntity equipEntity)
    {
        base.SetData(equipEntity);
        skillEntity = EquipEntity.weaponCombatEntities[0];
    }

    public override void Execute()
    {
        if (cooltime > 0) return;

        freezeTime = Defines.ATTACK_ANIMATION_SPEED_OFFSET / combatSlots.Condition.CurrentStat.attackSpeed;
        
        CombatData combat = new CombatData(skillEntity);
        cooltime = combat.cooltime;
        combatSlots.ChangeLayerWeight(EquipEntity.combatStyleType);
        combatSlots.Animator.SetFloat(combatSlots.AnimationData.AttackSpeedHash, combatSlots.Condition.CurrentStat.attackSpeed);
        combatSlots.Animator.SetTrigger(combatSlots.AnimationData.SkillHash);
        ApplyAttack(skillEntity.projectilePrefabPath, combat, null);
        combatSlots.IsSkillCasting = true;
    }
}