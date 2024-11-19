using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatSlots : MonoBehaviour
{
    public event Action OnTimerChanged;
    private Dictionary<Defines.CharacterAttackInputType, CombatBase> slots;
    [field: SerializeField] public LayerMask EnemyLayerMask { get; private set; }
    public Defines.CharacterCombatStyleType currentCombatStyleType { get; private set; }

    public Animator Animator { get; private set; }
    public IEquipable Equipment { get; private set; }
    public Condition Condition { get; private set; }
    public AnimationData AnimationData { get; private set; }

    private void Awake()
    {
        slots = new Dictionary<Defines.CharacterAttackInputType, CombatBase>();
    }

    private void Update()
    {
        OnTimerChanged?.Invoke();
    }

    public void Init(AnimationData data)
    {
        AnimationData = data;
        Animator = GetComponentInChildren<Animator>();
        Equipment = GetComponent<IEquipable>();
        Condition = GetComponent<Condition>();
    }

    public void AddSlot(Defines.CharacterAttackInputType attackInput, ItemEquipableEntity equipEntity)
    {
        CombatBase combatBase = null;
        if (attackInput == Defines.CharacterAttackInputType.None) return;
        else if (attackInput == Defines.CharacterAttackInputType.ComboAttack)
        {
            combatBase = new ComboAttack(this);
            combatBase.SetData(equipEntity);
        }
        else if (attackInput == Defines.CharacterAttackInputType.Skill)
        {
            combatBase = new SkillAttack(this);
            combatBase.SetData(equipEntity);
        }
        
        slots.Add(attackInput, combatBase);
    }

    public void RemoveSlot(Defines.CharacterAttackInputType attackInput)
    {
        if (slots.TryGetValue(attackInput, out CombatBase combatBase))
            combatBase.RemoveData();

        slots.Remove(attackInput);
    }

    public void Use(Defines.CharacterAttackInputType attackInput)
    {
        if (slots.TryGetValue(attackInput, out CombatBase combatBase))
        {
            combatBase.Use();
        }
        
    }

    public void UnUse()
    {
        // ??
    }

    public void ChangeLayerWeight(Defines.CharacterCombatStyleType combatStyleType)
    {
        if (currentCombatStyleType == combatStyleType) return;
        Animator.SetLayerWeight((int)currentCombatStyleType, 0);
        currentCombatStyleType = combatStyleType;
        Animator.SetLayerWeight((int)currentCombatStyleType, 1);
    }
}