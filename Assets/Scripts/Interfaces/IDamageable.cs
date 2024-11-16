using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage);
    public void Heal(float heal);
    public void Knockback(Vector3 direction, float force, float duration);
}