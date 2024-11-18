using System;

[Serializable]
public class JobEntity : EntityBase
{
     public string image;
     public string prefabPath;
     
     public JobStatEntity jobStatEntity;
}

[Serializable]
public class JobStatEntity
{
     public int jobId;
     public float maxHp;
     public float passiveHpRegen;
     public float maxMp;
     public float passiveMpRegen;
     public float attack;
     public float armor;
     public float attackSpeed;
     public float moveSpeed;
}