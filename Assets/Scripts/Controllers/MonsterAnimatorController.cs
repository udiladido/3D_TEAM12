using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterAnimatorController : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private string spawnName = "Spawn";
    [SerializeField] private string isSpawningName = "IsSpawning";
    [SerializeField] private string attractedName = "Attracted";
    [SerializeField] private string isTauntingName = "IsTaunting";
    [SerializeField] private string runName = "Run";
    [SerializeField] private string attackName = "Attack";
    [SerializeField] private string attackIndexName = "AttackIndex";
    [SerializeField] private string hitName = "Hit";
    [SerializeField] private string hitIndexName = "HitIndex";
    [SerializeField] private string isStaggerName = "IsStagger";
    [SerializeField] private string deathName = "Death";
    [SerializeField] private string deathIndexName = "DeathIndex";
    [SerializeField] private string isDeadName = "IsDead";

    private static bool hashLoad = false;
    private static int hashSpawnTrigger;
    private static int hashIsSpawning;
    private static int hashAttractedTrigger;
    private static int hashIsTaunting;
    private static int hashRunBool;
    private static int hashAttackTrigger;
    private static int hashAttackIndex;
    private static int hashHitTrigger;
    private static int hashHitIndex;
    private static int hashIsStagger;
    private static int hashDeathTrigger;
    private static int hashDeathIndex;
    private static int hashDeadBool;

    public event Action OnShot;
    public event Action OnSpawningEnd;
    public event Action OnTauntingEnd;
    public event Action OnHitEnd;
    public bool IsSpawning { get; private set; }
    public bool IsTaunting { get; private set; }
    public bool IsStagger { get; private set; }
    public bool IsDead { get; private set; }

    public void Initialize()
    {
        animator = GetComponent<Animator>();

        if (hashLoad == false)
        {
            hashLoad = true;

            hashSpawnTrigger = Animator.StringToHash(spawnName);
            hashIsSpawning = Animator.StringToHash(isSpawningName);
            hashAttractedTrigger = Animator.StringToHash(attractedName);
            hashIsTaunting = Animator.StringToHash(isTauntingName);
            hashRunBool = Animator.StringToHash(runName);
            hashAttackTrigger = Animator.StringToHash(attackName);
            hashAttackIndex = Animator.StringToHash(attackIndexName);
            hashHitTrigger = Animator.StringToHash(hitName);
            hashHitIndex = Animator.StringToHash(hitIndexName);
            hashIsStagger = Animator.StringToHash(isStaggerName);
            hashDeathTrigger = Animator.StringToHash(deathName);
            hashDeathIndex = Animator.StringToHash(deathIndexName);
            hashDeadBool = Animator.StringToHash(isDeadName);
        }

        OnShot = null;
        OnSpawningEnd = null;
        OnTauntingEnd = null;
        OnHitEnd = null;
        IsSpawning = false;
        IsTaunting = false;
        IsStagger = false;
        IsDead = false;
    }

    public bool TriggerSpawn()
    {
        if (IsSpawning || IsTaunting || IsDead)
        {
            return false;
        }

        IsSpawning = true;
        animator.SetBool(hashIsSpawning, true);
        animator.SetTrigger(hashSpawnTrigger);
        return true;
    }

    public bool TriggerTaunt()
    {
        if (IsSpawning || IsTaunting || IsDead)
        {
            return false;
        }

        IsTaunting = true;
        animator.SetBool(hashIsTaunting, true);
        animator.SetTrigger(hashAttractedTrigger);
        return true;
    }

    public void SetRun(bool run)
    {
        animator.SetBool(hashRunBool, run);
    }

    public bool TriggerAttack(int attackIndex)
    {
        if (IsSpawning || IsTaunting || IsDead)
        {
            return false;
        }

        animator.SetInteger(hashAttackIndex, attackIndex);
        animator.SetTrigger(hashAttackTrigger);
        return true;
    }

    public void TriggerHit()
    {
        int hitIndex = Random.Range(0, 2);
        IsStagger = true;
        animator.SetBool(hashIsStagger, true);
        animator.SetInteger(hashHitIndex, hitIndex);
        animator.SetTrigger(hashHitTrigger);
    }
    public void TriggerHit(int hitIndex)
    {
        IsStagger = true;
        animator.SetBool(hashIsStagger, true);
        animator.SetInteger(hashHitIndex, hitIndex);
        animator.SetTrigger(hashHitTrigger);
    }

    public void TriggerDeath()
    {
        int deathIndex = Random.Range(0, 3);
        IsDead = true;
        animator.SetBool(hashDeadBool, true);
        animator.SetInteger(hashDeathIndex, deathIndex);
        animator.SetTrigger(hashDeathTrigger);
    }
    public void TriggerDeath(int deathIndex)
    {
        IsDead = true;
        animator.SetBool(hashDeadBool, true);
        animator.SetInteger(hashDeathIndex, deathIndex);
        animator.SetTrigger(hashDeathTrigger);
    }


    // 애니메이션 이벤트에 등록
    private void ShotEvent()
    {
        OnShot?.Invoke();
    }
    private void SpawningEndEvent()
    {
        IsSpawning = false;
        animator.SetBool(hashIsSpawning, false);
        OnSpawningEnd?.Invoke();
    }
    private void TauntingEndEvent()
    {
        IsTaunting = false;
        animator.SetBool(hashIsTaunting, false);
        OnTauntingEnd?.Invoke();
    }
    private void HitEndEvent()
    {
        IsStagger = false;
        animator.SetBool(hashIsStagger, false);
        OnHitEnd?.Invoke();
    }
}
