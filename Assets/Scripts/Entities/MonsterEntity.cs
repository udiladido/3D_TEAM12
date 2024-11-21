using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class MonsterEntity : EntityBase
{
    public int level;
    public string prefabPath;
    public string iconPath;

    public float colliderRadius;
    public float colliderHeight;
    public float colliderCenterY;

    public float maxHp;
    public float armor;
    public float moveSpeed;
    public float staggerDamage; // 맞아도 주춤하지 않을 대미지 한계값

    public float attractDistance;
    public float chasePeriod;
    public List<MonsterSkillEntity> skillEntities;
}

[Serializable]
public class MonsterSkillEntity
{
    public int monsterId;
    public string projectilePrefabPath;
    public float selectWeight;

    public float attackRange;
    public float attackDamage;
    public float attackPeriod;
    public float duration;
    public float startScale;
    public float endScale;
    public float moveSpeed;
    public float waitTime;
    public float knockbackPower;
    public float knockbackDuration;

}
