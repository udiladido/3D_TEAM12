using System;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [field: SerializeField] public int CurrentComboAttackIndex { get; private set; }
    [field: SerializeField] public float ComboDuration { get; private set; }
    private float comboTimer;

    private Animator animator;

    private IEquipable Equipment;

    private ItemEquipableEntity comboWeaponEntity;
    private ItemEquipableEntity weaponEntity;

    private Defines.CharacterCombatStyleType currentCombatStyleType;
    private ItemWeaponCombatEntity currentWeaponCombatEntity;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        Equipment = GetComponent<IEquipable>();
    }
    private void Update()
    {
        CheckComboDuration();
    }

    public void Skill(int attackHash)
    {
        if (Equipment == null) return;

        weaponEntity = Equipment.GetWeaponInfo(Defines.CharacterAttackInputType.Skill);
        if (weaponEntity == null) return;
        ChangeLayerWeight(weaponEntity.combatStyleType);

        animator.SetTrigger(attackHash);
        var combat = weaponEntity.weaponCombatEntities[0];
        // TODO : 프로젝타일 발사

    }

    public void ComboAttack(int attackHash, int comboHash)
    {
        if (Equipment == null) return;
        comboWeaponEntity = Equipment.GetWeaponInfo(Defines.CharacterAttackInputType.ComboAttack);
        if (comboWeaponEntity == null) return;

        int index = GetNextComboAttackIndex(comboWeaponEntity.weaponCombatEntities);
        if (index == CurrentComboAttackIndex) return;
        
        var combat = comboWeaponEntity.weaponCombatEntities[index];

        comboTimer = Time.time;

        ChangeLayerWeight(comboWeaponEntity.combatStyleType);
        animator.SetTrigger(attackHash);
        animator.SetInteger(comboHash, CurrentComboAttackIndex + 1);
        CurrentComboAttackIndex = index;
        // TODO : 프로젝타일 발사
    }


    private void ChangeLayerWeight(Defines.CharacterCombatStyleType combatStyleType)
    {
        if (currentCombatStyleType == combatStyleType) return;
        animator.SetLayerWeight((int)currentCombatStyleType, 0);
        currentCombatStyleType = combatStyleType;
        animator.SetLayerWeight((int)currentCombatStyleType, 1);
        CurrentComboAttackIndex = 0;
    }

    public void SetAttackSpeed(int attackSpeedHash, float attackSpeed)
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat(attackSpeedHash, attackSpeed / Defines.ATTACK_ANIMATION_SPEED_OFFSET);
    }

    private void CheckComboDuration()
    {
        if (Time.time - comboTimer > ComboDuration)
        {
            // 콤보 시간이 지나면 콤보 인덱스를 초기화합니다.
            CurrentComboAttackIndex = 0;
        }
    }

    private bool CheckComboAttacking()
    {
        if (comboWeaponEntity == null)
        {
            return false;
        }
        var currentState = animator.GetCurrentAnimatorStateInfo((int)comboWeaponEntity.combatStyleType);
        if (currentState.IsTag("Attack"))
        {
            return currentState.normalizedTime < 1f;
        }

        return false;
    }

    private int GetNextComboAttackIndex(List<ItemWeaponCombatEntity> combatEntities)
    {
        if (CheckComboAttacking())
            return CurrentComboAttackIndex;
        
        if (CurrentComboAttackIndex + 1 >= combatEntities.Count)
            return 0;

        return CurrentComboAttackIndex + 1;
    }
}