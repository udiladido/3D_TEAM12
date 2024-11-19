using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCondition : MonoBehaviour, IDamageable
{
    public event Action OnHit;
    public event Action OnDead;

    public bool IsDead { get; private set; }
    private float maxHp;
    private float currentHp;
    private int nextAttackIndex;

    public void SetData(float _maxHp)
    {
        IsDead = false;
        maxHp = _maxHp;
        currentHp = maxHp;
        nextAttackIndex = 0;
    }

    public void TakeDamage(float damage)
    {
        currentHp = Mathf.Clamp(currentHp - damage, 0, maxHp);
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
        currentHp = Mathf.Clamp(currentHp + heal, 0, maxHp);
    }

    public void Knockback(Vector3 direction, float force, float duration)
    {
        // TODO : 넉백?
    }
}
