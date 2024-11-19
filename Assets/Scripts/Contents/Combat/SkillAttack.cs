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

    public override void Use()
    {
        if (cooltime > 0) return;
        
        combatSlots.ChangeLayerWeight(EquipEntity.combatStyleType);
        combatSlots.Animator.SetFloat(combatSlots.AnimationData.AttackSpeedHash, combatSlots.Condition.CurrentStat.attackSpeed);
        combatSlots.Animator.SetTrigger(combatSlots.AnimationData.SkillHash);
        CombatData combat = new CombatData(skillEntity);
        cooltime = combat.cooltime;
        ApplySkill(skillEntity.projectilePrefabPath, combat);
    }

    private void ApplySkill(string projectilePrefabPath, CombatData combatData, Transform parent = null)
    {
        GameObject go = Managers.Pool.Spawn(projectilePrefabPath, parent);
        if (go == null) return;
        ProjectileController projectile = go.GetComponent<ProjectileController>();
        projectile.SetData(combatSlots.transform, combatData, combatSlots.EnemyLayerMask);
        projectile.Launch();
    }
}