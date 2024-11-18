using System;
using UnityEngine;

/// <summary>
/// 가변 데이터를 가지는 스탯
/// </summary>
[Serializable]
public struct StatData
{
    public float maxHp;
    public float passiveHpRegen;
    public float maxMp;
    public float passiveMpRegen;
    public float attack;
    public float armor;
    public float attackSpeed;
    public float moveSpeed;

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
    }
}