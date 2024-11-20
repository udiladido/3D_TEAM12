using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 가변 데이터를 가지는 스탯
/// </summary>
[Serializable]
public class StatData
{
    public float maxHp;
    public float passiveHpRegen;
    public float maxMp;
    public float passiveMpRegen;
    public float attack;
    public float armor;
    public float attackSpeed;
    public float moveSpeed;
    public float cooltimeReduction; // 쿨타임 감소

    public StatModifier[] statModifiers;

    public StatData(JobStatEntity stat)
    {
        this.maxHp = stat.maxHp;
        this.passiveHpRegen = stat.passiveHpRegen;
        this.maxMp = stat.maxMp;
        this.passiveMpRegen = stat.passiveMpRegen;
        this.attack = stat.attack;
        this.armor = stat.armor;
        this.attackSpeed = stat.attackSpeed;
        this.moveSpeed = stat.moveSpeed;
        this.cooltimeReduction = stat.cooltimeReduction;
        
        List<StatModifier> items = new List<StatModifier>();
        items.Add(new StatModifier(Defines.CharacterStatType.Hp, Defines.CalcType.Override, stat.maxHp));
        items.Add(new StatModifier(Defines.CharacterStatType.Mp, Defines.CalcType.Override, stat.maxMp));
        items.Add(new StatModifier(Defines.CharacterStatType.AttackDamage, Defines.CalcType.Override, stat.attack));
        items.Add(new StatModifier(Defines.CharacterStatType.Armor, Defines.CalcType.Override, stat.armor));
        items.Add(new StatModifier(Defines.CharacterStatType.AttackSpeed, Defines.CalcType.Override, stat.attackSpeed));
        items.Add(new StatModifier(Defines.CharacterStatType.MoveSpeed, Defines.CalcType.Override, stat.moveSpeed));
        items.Add(new StatModifier(Defines.CharacterStatType.HpRegen, Defines.CalcType.Override, stat.passiveHpRegen));
        items.Add(new StatModifier(Defines.CharacterStatType.MpRegen, Defines.CalcType.Override, stat.passiveMpRegen));
        items.Add(new StatModifier(Defines.CharacterStatType.CooltimeReduction, Defines.CalcType.Override, stat.cooltimeReduction));
        statModifiers = items.ToArray();
    }

    public StatData(List<ItemStatBoostEffectEntity> boosts)
    {
        this.passiveHpRegen = 0;
        this.passiveMpRegen = 0;
        this.maxHp = 0;
        this.maxMp = 0;
        this.attack = 0;
        this.armor = 0;
        this.attackSpeed = 0;
        this.moveSpeed = 0;
        this.cooltimeReduction = 0;
        statModifiers = new StatModifier[boosts.Count];
        for (int i = 0; i < boosts.Count; i++)
        {
            ItemStatBoostEffectEntity stat = boosts[i];
            statModifiers[i] = new StatModifier(stat.statType, stat.calcType, stat.amount);
            switch (stat.statType)
            {
                case Defines.CharacterStatType.Hp:
                    this.maxMp = stat.amount;
                    break;
                case Defines.CharacterStatType.Mp:
                    this.maxMp = stat.amount;
                    break;
                case Defines.CharacterStatType.AttackDamage:
                    this.attack = stat.amount;
                    break;
                case Defines.CharacterStatType.Armor:
                    this.armor = stat.amount;
                    break;
                case Defines.CharacterStatType.AttackSpeed:
                    this.attackSpeed = stat.amount;
                    break;
                case Defines.CharacterStatType.MoveSpeed:
                    this.moveSpeed = stat.amount;
                    break;
                case Defines.CharacterStatType.HpRegen:
                    this.passiveHpRegen = stat.amount;
                    break;
                case Defines.CharacterStatType.MpRegen:
                    this.passiveMpRegen = stat.amount;
                    break;
                case Defines.CharacterStatType.CooltimeReduction:
                    this.cooltimeReduction = stat.amount;
                    break;
            }
        }
    }
    
    public void UpdateStat(Func<float, float, float> operation, StatModifier modifier)
    {
        switch (modifier.statType)
        {
            case Defines.CharacterStatType.Hp:
                maxHp = operation(maxHp, modifier.amount);
                break;
            case Defines.CharacterStatType.Mp:
                maxMp = operation(maxMp, modifier.amount);
                break;
            case Defines.CharacterStatType.AttackDamage:
                attack = operation(attack, modifier.amount);
                break;
            case Defines.CharacterStatType.Armor:
                armor = Mathf.Clamp(operation(armor, modifier.amount), 0, Defines.STAT_MAX_ARMOR);
                break;
            case Defines.CharacterStatType.AttackSpeed:
                attackSpeed = Mathf.Clamp(operation(attackSpeed, modifier.amount), 0.5f, Defines.STAT_MAX_ATTACK_SPEED);
                break;
            case Defines.CharacterStatType.MoveSpeed:
                moveSpeed = Mathf.Clamp(operation(moveSpeed, modifier.amount), 0, Defines.STAT_MAX_MOVE_SPEED);
                break;
            case Defines.CharacterStatType.HpRegen:
                passiveHpRegen = operation(passiveHpRegen, modifier.amount);
                break;
            case Defines.CharacterStatType.MpRegen:
                passiveMpRegen = operation(passiveMpRegen, modifier.amount);
                break;
            case Defines.CharacterStatType.CooltimeReduction:
                cooltimeReduction = Mathf.Clamp(operation(cooltimeReduction, modifier.amount), 1f, Defines.STAT_MAX_COOLTIME_REDUCE_RATE);
                break;
        }
    }
}

public struct StatModifier : IEquatable<StatModifier>
{
    public Defines.CharacterStatType statType;
    public Defines.CalcType calcType;
    public float amount;
    
    public StatModifier(Defines.CharacterStatType statType, Defines.CalcType calcType, float amount)
    {
        this.statType = statType;
        this.calcType = calcType;
        this.amount = amount;
    }
    
    public bool Equals(StatModifier other)
    {
        return statType == other.statType && calcType == other.calcType && amount.Equals(other.amount);
    }
    
    public override bool Equals(object obj)
    {
        return obj is StatModifier other && Equals(other);
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine((int)statType, (int)calcType, amount);
    }
}