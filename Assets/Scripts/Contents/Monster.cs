using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class Monster : MonoBehaviour
{
    // 전역 몬스터 설정
    public static Camera MainCamera;
    public static bool UnlimitRangeOfDetection;

    public int Identifier { get; private set; }

    public MonsterEntity Stat { get; private set; }
    public MonsterCondition Condition { get; private set; }
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
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image healthbar;

    private void Awake()
    {
        Condition = GetComponent<MonsterCondition>();
        Condition.OnHit += TakeDamage;
        Condition.OnDead += Die;
        HitCollider = GetComponent<CapsuleCollider>();
        RigidBody = GetComponent<Rigidbody>();

        stateMachine = new MonsterStateMachine(this);
    }


    public bool Initialize(int identifier, int monsterID, Vector3 spawnPoint)
    {
        Identifier = identifier;

        this.transform.localPosition = spawnPoint;
        MonsterEntity monsterEntity = Managers.DB.Get<MonsterEntity>(monsterID);
        if (monsterEntity == null) return false;
        GameObject go = Managers.Resource.Instantiate(monsterEntity.prefabPath, this.transform);
        if (go == null) return false;

        Stat = monsterEntity;
        Condition.SetData(Stat.maxHp);
        HitCollider.radius = Stat.colliderRadius;
        HitCollider.height = Stat.colliderHeight;
        HitCollider.center = new Vector3(0, Stat.colliderCenterY, 0);
        AnimationController = go.GetComponent<MonsterAnimatorController>();
        ValidAnimator = AnimationController != null;
        if (ValidAnimator)
        {
            AnimationController.Initialize();
            AnimationController.OnShot += Shot;
        }

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

        canvas.transform.localPosition = new Vector3(0, Stat.colliderHeight * 1.5f + 0.5f);
        canvas.transform.localScale = new Vector3(Stat.colliderRadius * 0.02f, Stat.colliderRadius * 0.02f, 0);
        canvas.enabled = false;

        return true;
    }

    private void Update()
    {
        stateMachine.Update();

        canvas.transform.LookAt(MainCamera.transform);
        healthbar.fillAmount = Condition.CurrentHp / Condition.MaxHp;
    }


    public void TakeDamage(float damage)
    {
        OnHit?.Invoke(damage > Stat.staggerDamage);
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

    public void ShowCanvas(bool show)
    {
        canvas.enabled = show;
    }


    private void Die()
    {
        canvas.enabled = false;
        stateMachine.ChangeState(stateMachine.State_Dead);
        OnDead?.Invoke(Identifier);
    }
    public void SetDisable()
    {
        ValidAnimator = false;
        if (AnimationController != null)
            Destroy(AnimationController.gameObject);
        AnimationController = null;

        OnDisabled?.Invoke(Identifier);

        OnHit = null;
        OnDead = null;
        OnDisabled = null;
        gameObject.SetActive(false);
    }

    public void Shot()
    {
        // TODO : Projectile 생성
        var skill = Stat.skillEntities[NextSkillIndex];
        GameObject go = Managers.Pool.Spawn(skill.projectilePrefabPath);
        MonsterProjectileController projectile = go.GetComponent<MonsterProjectileController>();
        Transform target = Managers.Game.Player.gameObject.transform;
        projectile.SetData(transform, target, skill, targetLayer);
        projectile.Launch();
    }
}