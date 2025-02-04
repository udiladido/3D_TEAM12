using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour, IDamageable, IStatHandler
{
    public event Action<float, float> OnHpChanged;
    public event Action<float, float> OnMpChanged;
    public event Action OnHpWarning;
    public event Action OnMpWarning;
    public event Action<StatData> OnStatChanged;
    public event Action OnDead;
    public event Action OnHit;
    public event Action<float, float> OnDodgeTimerChanged;

    [field: SerializeField] public StatData CurrentStat { get; private set; }
    private JobStatEntity statEntity;
    public float currentHp;
    public float currentMp;
    public bool IsDead;

    private StatData baseStat;
    private List<StatModifier> statsModifiers = new List<StatModifier>();

    // 무적 시간
    [SerializeField] private float invincibilityPeriod = 0.5f;
    private float invincibilityTimer;

    // 회피 소모 마나
    [SerializeField] private float dodgeCost = 10;
    [SerializeField] private float dodgeCooltime = 1f;
    private float dodgeTimer;

    private void Start()
    {
        currentHp = CurrentStat.maxHp;
        currentMp = CurrentStat.maxMp;
        FullRecovery();
    }

    public void SetData(JobStatEntity statEntity, bool isFullRecovery = false)
    {
        this.statEntity = statEntity;
        baseStat = new StatData(statEntity);
        OnStatChanged?.Invoke(CurrentStat);

        if (isFullRecovery) FullRecovery();
    }

    public void Update()
    {
        TimerUpdate();
        // 임시로 체력, 마나 바로 리젠 
        HpRegen();
        MpRegen();
    }
    
    private void TimerUpdate()
    {
        invincibilityTimer += Time.deltaTime;
        dodgeTimer += Time.deltaTime;
        OnDodgeTimerChanged?.Invoke(dodgeTimer, dodgeCooltime);
    }

    public void FullRecovery()
    {
        IsDead = false;
        currentHp = CurrentStat.maxHp;
        currentMp = CurrentStat.maxMp;
        OnHpChanged?.Invoke(currentHp, CurrentStat.maxHp);
        OnMpChanged?.Invoke(currentMp, CurrentStat.maxMp);
    }
    public void TakeDamage(float damage)
    {
        if (invincibilityTimer < invincibilityPeriod)
            return;

        invincibilityTimer = 0;
        float value = (1 - CurrentStat.armor * 0.01f) * damage;

        currentHp = Mathf.Clamp(currentHp - value, 0, CurrentStat.maxHp);
        OnHpChanged?.Invoke(currentHp, CurrentStat.maxHp);
        // TODO : 피격 이펙트
        // TODO : 피격 사운드
        if (currentHp <= 0)
        {
            IsDead = true;
            OnDead?.Invoke();
        }
        else
        {
            Managers.Sound.PlaySFX("Damaged", transform.position);
            OnHit?.Invoke();
            OnHpWarning?.Invoke();
        }
    }

    public void Heal(float heal)
    {
        currentHp = Mathf.Clamp(currentHp + heal, 0, CurrentStat.maxHp);
        OnHpChanged?.Invoke(currentHp, CurrentStat.maxHp);
    }

    public void MpRecovery(float recovery)
    {
        currentMp = Mathf.Clamp(currentMp + recovery, 0, CurrentStat.maxMp);
        OnMpChanged?.Invoke(currentMp, CurrentStat.maxMp);
    }

    private void HpRegen()
    {
        if (CurrentStat.passiveHpRegen > 0)
        {
            currentHp = Mathf.Clamp(currentHp + CurrentStat.passiveHpRegen * Time.deltaTime, 0, CurrentStat.maxHp);
            OnHpChanged?.Invoke(currentHp, CurrentStat.maxHp);
        }
    }

    private void MpRegen()
    {
        if (CurrentStat.passiveMpRegen > 0)
        {
            currentMp = Mathf.Clamp(currentMp + CurrentStat.passiveMpRegen * Time.deltaTime, 0, CurrentStat.maxMp);
            OnMpChanged?.Invoke(currentMp, CurrentStat.maxMp);
        }
    }

    public bool TryUseMana(float cost)
    {
        if (currentMp < cost)
        {
            OnMpWarning?.Invoke();
            return false;
        }

        currentMp = Mathf.Clamp(currentMp - cost, 0, CurrentStat.maxMp);
        OnMpChanged?.Invoke(currentMp, CurrentStat.maxMp);
        return true;
    }

    public bool TryDodge()
    {
        if (dodgeTimer < dodgeCooltime) return false;
        if (TryUseMana(dodgeCost) == false) return false;
        dodgeTimer = 0;
        return true;
    }

    public void Knockback(Vector3 direction, float force, float duration)
    {
        // TODO : 넉백?
    }

    public void AddStatModifier(params StatModifier[] modifiers)
    {
        statsModifiers.AddRange(modifiers);
        UpdateStatModifier();
    }
    public void RemoveStatModifier(params StatModifier[] modifiers)
    {
        foreach (var modifier in modifiers)
            statsModifiers.Remove(modifier);
        UpdateStatModifier();
    }

    public void UpdateStatModifier()
    {
        foreach (var modifier in baseStat.statModifiers)
        {
            ApplyStatModifier(modifier);
        }

        foreach (var modifier in statsModifiers)
        {
            ApplyStatModifier(modifier);
        }
    }
    private void ApplyStatModifier(StatModifier modifier)
    {
        CurrentStat.UpdateStat(Utils.Operation(modifier.calcType), modifier);
    }
}