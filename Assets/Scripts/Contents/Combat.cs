using System;
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

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        Equipment = GetComponent<IEquipable>();
    }
    private void Update()
    {
        if (Time.time - comboTimer > ComboDuration)
        {
            CurrentComboAttackIndex = 0;
        }
    }


    public void Attack(int attackHash)
    {
        if (Equipment == null) return;

        ItemEquipableEntity equipItem = Equipment.GetWeaponInfo(Defines.CharacterAttackInputType.Attack);
        if (equipItem == null) return;
        ChangeLayerWeight(equipItem.combatStyleType);
        
        animator.SetTrigger(attackHash);
    }

    public void ComboAttack(int attackHash, int comboHash)
    {
        if (Equipment == null) return;

        ItemEquipableEntity equipItem = Equipment.GetWeaponInfo(Defines.CharacterAttackInputType.ComboAttack);
        if (equipItem == null) return;
        ChangeLayerWeight(equipItem.combatStyleType);
        
        comboTimer = Time.time;
        CurrentComboAttackIndex++;

        animator.SetTrigger(attackHash);
        animator.SetInteger(comboHash, CurrentComboAttackIndex);
        
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