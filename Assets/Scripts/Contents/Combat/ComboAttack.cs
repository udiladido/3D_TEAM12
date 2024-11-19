using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class ComboAttack : CombatBase
{
    private List<ItemWeaponCombatEntity> comboAttacks => EquipEntity.weaponCombatEntities;

    private int currentComboIndex = -1;
    private int nextComboIndex = -1;

    private float comboAttackTime;
    private float resetComboTime = 0.5f;

    private bool cancelable => currentComboIndex > -1 && comboAttacks.Count > currentComboIndex;

    public ComboAttack(CombatSlots combatSlots) : base(combatSlots)
    {

    }

    public override void Update()
    {
        base.Update();

        if (cancelable && Time.time - comboAttackTime > resetComboTime)
        {
            currentComboIndex = -1;
            nextComboIndex = -1;
            combatSlots.Animator.SetInteger(combatSlots.AnimationData.ComboAttackIndexHash, 0);
            return;
        }

        if (nextComboIndex > -1 && cooltime <= 0)
        {
            Attack(nextComboIndex);
        }
    }

    public override void Use()
    {
        nextComboIndex = GetNextComboIndex(currentComboIndex);
        comboAttackTime = Time.time;
    }

    private void Attack(int comboIndex)
    {
        if (comboIndex == -1) return;
        ItemWeaponCombatEntity combatEntity = comboAttacks[comboIndex];
        CombatData combat = new CombatData(combatEntity);
        cooltime = combatEntity.cooltime * Defines.ATTACK_ANIMATION_SPEED_OFFSET / combatSlots.Condition.CurrentStat.attackSpeed;
        combatSlots.ChangeLayerWeight(EquipEntity.combatStyleType);
        combatSlots.Animator.SetFloat(combatSlots.AnimationData.AttackSpeedHash, combatSlots.Condition.CurrentStat.attackSpeed);
        combatSlots.Animator.SetTrigger(combatSlots.AnimationData.ComboAttackHash);
        combatSlots.Animator.SetInteger(combatSlots.AnimationData.ComboAttackIndexHash, comboIndex + 1);
        ApplyAttack(combatEntity.projectilePrefabPath, combat, combatSlots.transform);

        currentComboIndex = comboIndex;
    }

    private int GetNextComboIndex(int comboIndex)
    {
        if (comboAttacks.Count <= comboIndex + 1)
            return 0;

        return comboIndex + 1;
    }
}