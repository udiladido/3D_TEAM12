using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage);
    public void Heal(int heal);
    public void Knockback(Vector3 direction, float force, float duration);
    public void Die();
}