using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MonsterCondition : MonoBehaviour, IDamageable
{
    public event Action<float> OnHit;
    public event Action OnDead;

    [field: SerializeField] public bool IsDead { get; private set; }
    [field: SerializeField] public float MaxHp { get; private set; }
    [field: SerializeField] public float CurrentHp { get; private set; }

    public void SetData(float _maxHp)
    {
        IsDead = false;
        MaxHp = _maxHp;
        CurrentHp = MaxHp;
    }

    public void TakeDamage(float damage)
    {
        CurrentHp = Mathf.Clamp(CurrentHp - damage, 0, MaxHp);
        // TODO : 피격 이펙트
        // TODO : 피격 사운드
        if (CurrentHp <= 0)
        {
            IsDead = true;
            OnDead?.Invoke();
        }
        else
        {
            OnHit?.Invoke(damage);
        }
    }

    public void Heal(float heal)
    {
        CurrentHp = Mathf.Clamp(CurrentHp + heal, 0, MaxHp);
    }

    public void Knockback(Vector3 direction, float force, float duration)
    {
        // TODO : 넉백?
    }
}
