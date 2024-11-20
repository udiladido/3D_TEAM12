using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatSlots : MonoBehaviour
{
    public event Action OnTimerChanged;
    public event Action OnUpdate;
    private Dictionary<Defines.CharacterAttackInputType, CombatBase> slots;
    [field: SerializeField] public LayerMask EnemyLayerMask { get; private set; }
    public Defines.CharacterCombatStyleType currentCombatStyleType { get; private set; }

    public bool IsSkillCasting { get; set; }
    public Animator Animator { get; private set; }
    public IEquipment Equipment { get; private set; }
    public Condition Condition { get; private set; }
    public AnimationData AnimationData { get; private set; }

    private bool isComboAttaking;

    private void Awake()
    {
        slots = new Dictionary<Defines.CharacterAttackInputType, CombatBase>();
    }

    private void Update()
    {
        OnTimerChanged?.Invoke();
        OnUpdate?.Invoke();
        if (isComboAttaking && Equipment.CanAction(Defines.CharacterAttackInputType.ComboAttack))
            TryExecute(Defines.CharacterAttackInputType.ComboAttack);
    }

    public void Init(Animator animator, AnimationData data)
    {
        AnimationData = data;
        Animator = animator;
        Equipment = GetComponent<IEquipment>();
        Condition = GetComponent<Condition>();
    }

    public void AddSlot(Defines.CharacterAttackInputType attackInput, ItemEquipableEntity equipEntity)
    {
        RemoveSlot(attackInput);

        CombatBase combatBase = null;
        if (attackInput == Defines.CharacterAttackInputType.None) return;
        else if (attackInput == Defines.CharacterAttackInputType.ComboAttack)
        {

            combatBase = new ComboAttack(this);
            combatBase.SetData(equipEntity);
            OnUpdate += combatBase.Update;
        }
        else if (attackInput == Defines.CharacterAttackInputType.Skill)
        {
            combatBase = new SkillAttack(this);
            combatBase.SetData(equipEntity);
            OnUpdate += combatBase.Update;
        }

        slots.Add(attackInput, combatBase);
    }

    public void RemoveSlot(Defines.CharacterAttackInputType attackInput)
    {
        if (slots.TryGetValue(attackInput, out CombatBase combatBase))
        {
            OnUpdate -= combatBase.Update;
            combatBase.RemoveData();
        }

        slots.Remove(attackInput);
    }

    public void Use(Defines.CharacterAttackInputType attackInput)
    {
        if (attackInput == Defines.CharacterAttackInputType.ComboAttack)
            isComboAttaking = true;
        else
        {
            if (Equipment.CanAction(attackInput))
                TryExecute(attackInput);
        }
    }

    public void UnUse()
    {
        // ??
        isComboAttaking = false;
    }

    private void TryExecute(Defines.CharacterAttackInputType attackInput)
    {
        if (slots.TryGetValue(attackInput, out CombatBase combatBase))
        {
            combatBase.Execute();
        }
    }

    public void ChangeLayerWeight(Defines.CharacterCombatStyleType combatStyleType)
    {
        if (currentCombatStyleType == combatStyleType) return;
        Animator.SetLayerWeight((int)currentCombatStyleType, 0);
        currentCombatStyleType = combatStyleType;
        Animator.SetLayerWeight((int)currentCombatStyleType, 1);
    }
}