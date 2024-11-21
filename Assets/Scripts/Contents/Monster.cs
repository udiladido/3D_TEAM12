using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class Monster : MonoBehaviour
{
    public int Identifier { get; private set; }

    public MonsterEntity Stat { get; private set; }
    [field: SerializeField] public MonsterCondition Condition { get; private set; }
    public CapsuleCollider HitCollider { get; private set; }
    public Rigidbody RigidBody { get; private set; }
    public MonsterAnimatorController AnimationController { get; private set; }

    private MonsterStateMachine stateMachine;

    //Event
    /// <summary>
    /// 매개변수 : IsStagger
    /// </summary>
    public event Action<bool> OnHit;
    /// <summary>
    /// 매개변수 : Identifier
    /// </summary>
    public event Action<int> OnDead;
    /// <summary>
    /// 매개변수 : Identifier
    /// </summary>
    public event Action<int> OnDisabled;

    private int skillNum;
    private float skillWeightTotal;
    public int NextSkillIndex { get; private set; }
    public float NextSkillRange { get; private set; }

    public bool ValidAnimator { get; private set; }
    public bool ValidSkill { get; private set; }

    private void Awake()
    {
        Condition = new MonsterCondition();

        Condition.OnDead += Die;
        HitCollider = GetComponent<CapsuleCollider>();
        RigidBody = GetComponent<Rigidbody>();

        stateMachine = new MonsterStateMachine(this);
    }


    public bool Initialize(int identifier, int monsterID)
    {
        Identifier = identifier;
        
        MonsterEntity monsterEntity = Managers.DB.Get<MonsterEntity>(monsterID);
        if (monsterEntity == null) return false;
        GameObject go = Managers.Resource.Instantiate(monsterEntity.prefabPath, this.transform);
        if (go == null) return false;

        Stat = monsterEntity;
        Condition.SetData(Stat.maxHp);
        HitCollider.radius = Stat.colliderRadius;
        HitCollider.center = new Vector3(0, Stat.colliderCenterY, 0);
        AnimationController = GetComponentInChildren<MonsterAnimatorController>();
        ValidAnimator = AnimationController != null;
        if (ValidAnimator) AnimationController.Initialize();

        stateMachine.ChangeState(stateMachine.State_Spawning);

        // 스킬 데이터
        if (Stat.skillEntities != null && Stat.skillEntities.Count > 0)
        {
            skillNum = Stat.skillEntities.Count;
            NextSkillIndex = 0;
            NextSkillRange = Stat.skillEntities[0].attackRange;

            for (int i = 0; i < Stat.skillEntities.Count; i++)
            {
                skillWeightTotal += Stat.skillEntities[i].selectWeight;
            }

            ValidSkill = true;
        }
        else
        {
            Debug.LogError($"몬스터의 스킬 정보가 없습니다. (id : {Stat.id}, Name : {Stat.displayTitle})");
            skillNum = 0;
            NextSkillIndex = 0;
            NextSkillRange = 0;

            ValidSkill = false;
        }
        if (ValidSkill) SetNextSkill();

        return true;
    }

    private void Update()
    {
        stateMachine.Update();
    }
    

    public void TakeDamage(float damage)
    {
        Condition.TakeDamage(damage);
        if (Condition.IsDead == false)
        {
            OnHit?.Invoke(damage > Stat.staggerDamage);
        }
    }


    public void SetNextSkill()
    {
        if (skillNum <= 0) return;

        float skillWeightSelect = Random.Range(0, skillWeightTotal);
        float skillWeightCumulative = 0;
        for (int i = 0; i < Stat.skillEntities.Count; i++)
        {
            skillWeightCumulative += Stat.skillEntities[i].selectWeight;
            if (skillWeightSelect < skillWeightCumulative)
            {
                NextSkillIndex = i;
                break;
            }
        }
    }


    private void Die()
    {
        stateMachine.ChangeState(stateMachine.State_Dead);
        OnDead?.Invoke(Identifier);
    }
    public void SetDisable()
    {
        ValidAnimator = false;
        AnimationController = null;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        OnDisabled?.Invoke(Identifier);

        OnHit = null;
        OnDead = null;
        OnDisabled = null;
        gameObject.SetActive(false);
    }
}
