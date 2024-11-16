using System;
using UnityEngine;

public class Condition : MonoBehaviour, IDamageable
{
    public event Action<float, float> OnHpChanged;
    public event Action<float, float> OnMpChanged;
    public event Action<StatData> OnStatChanged;
    public event Action OnDead;
    public event Action OnHit; 

    [field: SerializeField] public StatData CurrentStat { get; private set; }

    public float currentHp;
    public float currentMp;
    public bool IsDead;

    private void Start()
    {
        currentHp = CurrentStat.maxHp;
        currentMp = CurrentStat.maxMp;
        FullRecovery();
    }

    public void SetData(StatData statData, bool isFullRecovery = false)
    {
        CurrentStat = statData;
        OnStatChanged?.Invoke(CurrentStat);

        if (isFullRecovery) FullRecovery();
    }

    public void Update()
    {
        // 임시로 체력, 마나 바로 리젠 
        HpRegen();
        MpRegen();
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
        currentHp = Mathf.Clamp(currentHp - damage, 0, CurrentStat.maxHp);
        // TODO : 피격 이펙트
        // TODO : 피격 사운드
        if (currentHp <= 0)
        {
            IsDead = true;
            OnDead?.Invoke();
        }
        else
        {
            OnHit?.Invoke();
        }
    }

    public void Heal(float heal)
    {
        currentHp = Mathf.Clamp(currentHp + heal, 0, CurrentStat.maxHp);
    }

    private void HpRegen()
    {
        if (CurrentStat.passiveHpRegen > 0)
            currentHp = Mathf.Clamp(currentHp + CurrentStat.passiveHpRegen * Time.deltaTime, 0, CurrentStat.maxHp);
    }

    private void MpRegen()
    {
        if (CurrentStat.passiveMpRegen > 0)
            currentMp = Mathf.Clamp(currentMp + CurrentStat.passiveMpRegen * Time.deltaTime, 0, CurrentStat.maxMp);
    }


    public void Knockback(Vector3 direction, float force, float duration)
    {
        // TODO : 넉백?
    }
}