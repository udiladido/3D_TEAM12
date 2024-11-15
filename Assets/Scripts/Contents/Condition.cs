using System;
using UnityEngine;

public class Condition : MonoBehaviour, IDamageable
{
    public event Action<float, float> OnHpChanged;
    public event Action<float, float> OnMpChanged;
    public event Action<StatData> OnStatChanged;

    [field: SerializeField] public StatData CurrentStat { get; private set; }

    public float currentHp;
    public float currentMp;
    public bool IsDead => currentHp <= 0;

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
        currentHp = CurrentStat.maxHp;
        currentMp = CurrentStat.maxMp;
        OnHpChanged?.Invoke(currentHp, CurrentStat.maxHp);
        OnMpChanged?.Invoke(currentMp, CurrentStat.maxMp);
    }
    public void TakeDamage(int damage)
    {
        currentHp = Mathf.Max(currentHp - damage, 0);
        // TODO : 피격 이펙트
        // TODO : 피격 사운드
        if (IsDead)
        {
            Die();
            return;
        }
    }

    public void Heal(int heal)
    {
        currentHp = Mathf.Min(currentHp + heal, CurrentStat.maxHp);
    }

    private void HpRegen()
    {
        if (CurrentStat.passiveHpRegen > 0)
            currentHp = Mathf.Min(currentHp + CurrentStat.passiveHpRegen * Time.deltaTime, CurrentStat.maxHp);
    }

    private void MpRegen()
    {
        if (CurrentStat.passiveMpRegen > 0)
            currentMp = Mathf.Min(currentMp + CurrentStat.passiveMpRegen * Time.deltaTime, CurrentStat.maxMp);
    }


    public void Knockback(Vector3 direction, float force, float duration)
    {
        // TODO : 넉백?
    }

    public void Die()
    {
        // TODO : 사망
    }
}