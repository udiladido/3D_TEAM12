using System;
using UnityEngine;

public class Condition : MonoBehaviour, IDamageable
{
    [field: SerializeField] public StatData CurrentStat { get; private set; }

    public float currentHp;
    public float currentMp;
    public bool IsDead => currentHp <= 0;

    public void Update()
    {
        // 임시로 체력, 마나 바로 리젠 
        if (CurrentStat.passiveHpRegen > 0)
            currentHp += CurrentStat.passiveHpRegen * Time.deltaTime;
        if (CurrentStat.passiveMpRegen > 0)
            currentMp += CurrentStat.passiveMpRegen * Time.deltaTime;
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
    
    public void Knockback(Vector3 direction, float force, float duration)
    {
        // TODO : 넉백?
    }

    public void Die()
    {
        // TODO : 사망
    }
}