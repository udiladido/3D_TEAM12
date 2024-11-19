using System;
using System.Collections.Generic;

[Serializable]
public class MonsterEntity : EntityBase
{
    public int level;
    public string prefabPath;
    public string iconPath;

    public float colliderRadius;
    public float colliderCenterY;

    public float maxHp;
    public float armor;
    public float moveSpeed;
    public float attractDistance;

    public List<MonsterAttack> monsterAttackList;
    //public List<MonsterDropItemEntity> MonsterDropItemEntities;
}

[Serializable]
public class MonsterAttack
{
    public string projectilePrefabPath;
    public float selectWeight;

    public float attackRange;
    public float attackDamage;
    public float attackSpeed;
}

[Serializable]
public class MonsterDropItemEntity
{
    public int monsterId;
    public int itemId;
    public int dropRate;
}