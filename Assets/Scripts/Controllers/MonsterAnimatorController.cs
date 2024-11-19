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
    [SerializeField] private string deathName = "Death";
    [SerializeField] private string deathIndexName = "DeathIndex";
    [SerializeField] private string isDeadName = "IsDead";

    private int hashSpawnTrigger;
    private int hashIsSpawning;
    private int hashAttractedTrigger;
    private int hashIsTaunting;
    private int hashRunBool;
    private int hashAttackTrigger;
    private int hashAttackIndex;
    private int hashHitTrigger;
    private int hashHitIndex;
    private int hashDeathTrigger;
    private int hashDeathIndex;
    private int hashDeadBool;

    public event Action OnShot;
    public event Action OnSpawningEnd;
    public event Action OnTauntingEnd;
    public bool IsSpawning { get; private set; }
    public bool IsTaunting { get; private set; }
    public bool IsDead { get; private set; }

    public void Initialize()
    {
        animator = GetComponent<Animator>();

        hashSpawnTrigger = Animator.StringToHash(spawnName);
        hashIsSpawning = Animator.StringToHash(isSpawningName);
        hashAttractedTrigger = Animator.StringToHash(attractedName);
        hashIsTaunting = Animator.StringToHash(isTauntingName);
        hashRunBool = Animator.StringToHash(runName);
        hashAttackTrigger = Animator.StringToHash(attackName);
        hashAttackIndex = Animator.StringToHash(attackIndexName);
        hashHitTrigger = Animator.StringToHash(hitName);
        hashHitIndex = Animator.StringToHash(hitIndexName);
        hashDeathTrigger = Animator.StringToHash(deathName);
        hashDeathIndex = Animator.StringToHash(deathIndexName);
        hashDeadBool = Animator.StringToHash(isDeadName);
    }

    public bool Spawn()
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

    public bool Taunt()
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

    public bool Attack(int attackIndex)
    {
        if (IsSpawning || IsTaunting || IsDead)
        {
            return false;
        }

        animator.SetInteger(hashAttackIndex, attackIndex);
        animator.SetTrigger(hashAttackTrigger);
        return true;
    }

    public void Hit()
    {
        int hitIndex = Random.Range(0, 2);
        animator.SetInteger(hashHitIndex, hitIndex);
        animator.SetTrigger(hashHitTrigger);
    }
    public void Hit(int hitIndex)
    {
        animator.SetInteger(hashHitIndex, hitIndex);
        animator.SetTrigger(hashHitTrigger);
    }

    public void Death()
    {
        int deathIndex = Random.Range(0, 3);
        IsDead = true;
        animator.SetBool(hashDeadBool, true);
        animator.SetInteger(hashDeathIndex, deathIndex);
        animator.SetTrigger(hashDeathTrigger);
    }
    public void Death(int deathIndex)
    {
        IsDead = true;
        animator.SetBool(hashDeadBool, true);
        animator.SetInteger(hashDeathIndex, deathIndex);
        animator.SetTrigger(hashDeathTrigger);
    }



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
}
