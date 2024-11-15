using System;

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
    public float moveSpeed;
    public float attackSpeed;
}