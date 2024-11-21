using System;
using UnityEngine;

public class MonsterProjectileController : ProjectileBaseController
{
    private MonsterSkillEntity skillEntity;
    private Transform owner;
    private Transform target;
    private LayerMask targetLayer;

    private Vector3 direction;
    private bool isLaunched;
    
    private float durationTimer;

    private void Update()
    {
        if (isLaunched)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > skillEntity.duration)
                DestroySelf();
            
            if (skillEntity.moveSpeed > 0)
                transform.position += direction * skillEntity.moveSpeed * Time.deltaTime;
        }
    }

    public override void SetData(Transform owner, Transform target, MonsterSkillEntity skillEntity, LayerMask targetLayer)
    {
        this.owner = owner;
        this.target = target;
        this.skillEntity = skillEntity;
        this.targetLayer = targetLayer;
        isLaunched = false;
        transform.position = owner.position;
        direction = (target.position - owner.position).normalized;
        durationTimer = 0;
    }

    public override void Launch()
    {
        Invoke(nameof(ApplyLaunch), skillEntity.waitTime);
    }
    
    protected override void ApplyLaunch()
    {
        // Do nothing
        isLaunched = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (Utils.LayerMaskContains(targetLayer, other.gameObject.layer))
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(skillEntity.attackDamage);
                DestroySelf();
            }
        }
    }
}