using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : ProjectileBaseController
{
    private CombatData projectileData;
    private Transform owner;

    private Vector3 direction;

    private bool isLaunched;
    private CombatBase combat;
    private Condition condition;

    private List<IDamageable> hitTargets = new List<IDamageable>();
    private float hitTimer = 0f;
    
    private float damage = 0;

    private float durationTimer;
    [SerializeField] private LayerMask targetLayer;


    private void Update()
    {
        if (isLaunched)
        {
            if (Time.time - durationTimer > projectileData.duration)
                DestroySelf();

            Move();
            Scale(projectileData.startScale, projectileData.endScale, projectileData.duration);

            if (Time.time - hitTimer > projectileData.hitInterval)
            {
                hitTimer = Time.time;
                Hits();
            }
        }
    }

    private void Hits()
    {
        if (hitTargets.Count == 0) return;
        float damage = condition.CurrentStat.attack * (projectileData.damagePer / 100);
        Vector3 knockbackDirection = transform.forward;
        foreach (var target in hitTargets)
            Hit(target, damage, knockbackDirection);
    }

    private void Hit(IDamageable target, float damage, Vector3 knockbackDirection)
    {
        target.TakeDamage(damage);
        if (projectileData.knockbackPower > 0)
            target.Knockback(knockbackDirection, projectileData.knockbackPower, projectileData.knockbackDuration);
            
        if (projectileData.pierceCount > 0 && ++hitCounter > projectileData.pierceCount)
        {
            DestroySelf();
        }
    }

    public override void SetData(Transform owner, CombatData projectileData, LayerMask enemyLayerMask)
    {
        SetData(owner, projectileData, enemyLayerMask, owner.forward);
    }

    public override void SetData(Transform owner, CombatData projectileData, LayerMask enemyLayerMask, Vector3 direction)
    {
        this.projectileData = projectileData;
        this.owner = owner;
        hitTargets.Clear();
        isLaunched = false;
        EnableHitBox(false);
        hitCounter = 0;
        transform.localScale = new Vector3(this.projectileData.startScale, this.projectileData.startScale, this.projectileData.startScale);
        ParticleShapeScale(transform.localScale);
        transform.position = owner.position + Vector3.up;
        transform.rotation = Quaternion.LookRotation(direction);
        this.direction = direction.normalized;
        condition = owner.GetComponent<Condition>();
        targetLayer = enemyLayerMask;
        damage = condition.CurrentStat.attack * (projectileData.damagePer / 100);
    }
    

    public override void Launch()
    {
        if (owner == null) return;
        // TODO : 발사
        float waitTime = projectileData.waitTime / condition.CurrentStat.attackSpeed;
        Invoke(nameof(ApplyLaunch), waitTime);
    }

    protected override void ApplyLaunch()
    {
        EnableHitBox(true);
        durationTimer = Time.time;
        isLaunched = true;
        Managers.Sound.PlaySFX(projectileData.sfxPath, transform.position);
    }

    private void Move()
    {
        if (projectileData.moveSpeed > 0)
        {
            transform.position += direction * projectileData.moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO : 충돌 처리
        if (Utils.LayerMaskContains(targetLayer, other.gameObject.layer))
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                float damage = condition.CurrentStat.attack * (projectileData.damagePer / 100);
                Hit(damageable, damage, transform.forward);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Utils.LayerMaskContains(targetLayer, other.gameObject.layer))
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                hitTargets.Remove(damageable);
            }
        }
    }
}