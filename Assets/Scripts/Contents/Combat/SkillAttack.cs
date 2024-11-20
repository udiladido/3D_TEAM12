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

    public override void SetData(Defines.CharacterAttackInputType attackInputType, ItemEquipableEntity equipEntity)
    {
        base.SetData(attackInputType, equipEntity);
        skillEntity = EquipEntity.weaponCombatEntities[0];
    }

    public override void Execute()
    {
        if (cooltime > 0) return;
        
        if (combatSlots.Condition.TryUseMana(skillEntity.manaCost) == false)
        {
            // TODO : 마나 부족
            return;
        }

        freezeTime = Defines.ATTACK_ANIMATION_SPEED_OFFSET / combatSlots.Condition.CurrentStat.attackSpeed;

        CombatData combat = new CombatData(skillEntity);
        float cooltimeReduction = combatSlots.Condition.CurrentStat.cooltimeReduction;
        // 쿨타임 감소 연산 공식 [스킬쿨타임] * ([쿨타임감소수치] - 1)
        maxCooltime = cooltimeReduction > 1 ? combat.cooltime * (cooltimeReduction - 1) : combat.cooltime;
        cooltime = maxCooltime;
        combatSlots.ChangeLayerWeight(EquipEntity.combatStyleType);
        combatSlots.Animator.SetFloat(combatSlots.AnimationData.AttackSpeedHash, combatSlots.Condition.CurrentStat.attackSpeed);
        combatSlots.Animator.SetTrigger(combatSlots.AnimationData.SkillHash);
        ApplyAttack(skillEntity.projectilePrefabPath, combat, null);
        combatSlots.IsSkillCasting = true;
    }

    protected override void ApplyAttack(string projectilePrefabPath, CombatData combatData, Transform parent = null)
    {
        if (combatData.numberOfProjectilePerShot == 1)
        {
            base.ApplyAttack(projectilePrefabPath, combatData, parent);
            return;
        }

        float projectileAngleSpace = combatData.multipleProjectilesAngle;
        float numberOfProjectilePerShot = combatData.numberOfProjectilePerShot;

        float angleStep = projectileAngleSpace / (numberOfProjectilePerShot - 1);
        float startAngle = -projectileAngleSpace / 2;

        for (int i = 0; i < numberOfProjectilePerShot; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * combatSlots.transform.forward;
            GameObject go = Managers.Pool.Spawn(projectilePrefabPath, parent);
            if (go == null) return;
            ProjectileController projectile = go.GetComponent<ProjectileController>();
            projectile.SetData(combatSlots.transform, combatData, combatSlots.EnemyLayerMask, direction);
            projectile.Launch();
        }
    }
}