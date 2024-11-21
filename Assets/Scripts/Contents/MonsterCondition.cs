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
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;

    public void SetData(float _maxHp)
    {
        IsDead = false;
        maxHp = _maxHp;
        currentHp = maxHp;
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
            OnHit?.Invoke(damage);
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
