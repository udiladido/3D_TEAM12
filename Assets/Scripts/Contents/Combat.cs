using System;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public event Action<float> OnSkillCooltimeChanged;
    
    [field: SerializeField] public int CurrentComboAttackIndex { get; private set; }
    [field: SerializeField] public float ComboDuration { get; private set; }
    [field: SerializeField] public bool IsAttacking { get; private set; }
    [field: SerializeField] public LayerMask EnemyLayerMask { get; private set; }
    private float comboTimer;

    private Animator animator;

    private IEquipable Equipment;
    private Condition condition;

    private ItemEquipableEntity comboWeaponEntity;
    private ItemEquipableEntity weaponEntity;

    private Defines.CharacterCombatStyleType currentCombatStyleType;

    private float skillCooltime;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        Equipment = GetComponent<IEquipable>();
        condition = GetComponent<Condition>();
    }
    private void Update()
    {
        CheckSkillCooltime();
    }

    private void CheckSkillCooltime()
    {
        if (skillCooltime <= 0) return;
        skillCooltime = Mathf.Max(0, skillCooltime - Time.deltaTime);
        OnSkillCooltimeChanged?.Invoke(skillCooltime);
    }

    public void Skill(int attackHash, int attackSpeedHash)
    {
        if (skillCooltime > 0) return;
        if (Equipment == null) return;
        weaponEntity = Equipment.GetWeaponInfo(Defines.CharacterAttackInputType.Skill);
        if (weaponEntity == null) return;

        ChangeLayerWeight(weaponEntity.combatStyleType);
        animator.SetFloat(attackSpeedHash, condition.CurrentStat.attackSpeed);
        animator.SetTrigger(attackHash);
        ItemWeaponCombatEntity combat = weaponEntity.weaponCombatEntities[0];
        skillCooltime = combat.cooltime;
        DoSkill(combat);
    }

    public void ComboAttack(int attackHash, int comboHash, int attackSpeedHash)
    {
        if (Equipment == null) return;
        comboWeaponEntity = Equipment.GetWeaponInfo(Defines.CharacterAttackInputType.ComboAttack);
        if (comboWeaponEntity == null) return;
        IsAttacking = true;
        
        // TODO : 어떻게 할건지..
    }

    private void DoSkill(ItemWeaponCombatEntity combatEntity, Transform parent = null)
    {
        GameObject go = Managers.Pool.Spawn(combatEntity.projectilePrefabPath, parent);
        if (go == null) return;
        ProjectileController projectile = go.GetComponent<ProjectileController>();
        projectile.SetData(transform, combatEntity);
        projectile.Launch();
    }

    private void DoComboAttack()
    {

    }

    public void AttackCancel()
    {
        IsAttacking = false;
    }


    private void ChangeLayerWeight(Defines.CharacterCombatStyleType combatStyleType)
    {
        if (currentCombatStyleType == combatStyleType) return;
        animator.SetLayerWeight((int)currentCombatStyleType, 0);
        currentCombatStyleType = combatStyleType;
        animator.SetLayerWeight((int)currentCombatStyleType, 1);
        CurrentComboAttackIndex = 0;
    }

}