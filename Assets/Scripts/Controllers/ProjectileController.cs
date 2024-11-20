using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileController : BaseController
{
    private CombatData projectileData;
    private Transform owner;

    private Vector3 direction;

    private ParticleSystem parentParticleSystem;
    private ParticleSystem[] particleSystems;
    private MeshRenderer meshRenderer;
    private Collider hitBoxCollider;

    private bool isLaunched;
    private CombatBase combat;
    private Condition condition;

    private float timer;
    [SerializeField] private LayerMask targetLayer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        hitBoxCollider = GetComponentInChildren<Collider>();
        parentParticleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (isLaunched)
        {
            if (Time.time - timer > projectileData.duration)
                DestroySelf();

            Move();
            Scale();

        }
    }

    private void EnableHitBox(bool enable)
    {
        if (hitBoxCollider != null)
            hitBoxCollider.enabled = enable;
        if (meshRenderer != null)
            meshRenderer.enabled = enable;
        if (parentParticleSystem != null)
            parentParticleSystem.gameObject.SetActive(enable);
    }

    private void Scale()
    {
        float scale = projectileData.endScale - projectileData.startScale;
        if (scale == 0) return;
        Vector3 scaleVector = Vector3.one * scale * Time.deltaTime / projectileData.duration;
        transform.localScale += scaleVector;
        ParticleShapeScale(transform.localScale);
    }
    
    private void ParticleShapeScale(Vector3 scaleVector)
    {
        foreach (var particle in particleSystems)
        {
            ParticleSystem.ShapeModule shape = particle.shape;
            shape.scale = scaleVector;
        }
    }

    public void SetData(Transform owner, CombatData projectileData, LayerMask enemyLayerMask)
    {
        this.projectileData = projectileData;
        this.owner = owner;
        isLaunched = false;
        EnableHitBox(false);

        transform.localScale = new Vector3(this.projectileData.startScale, this.projectileData.startScale, this.projectileData.startScale);
        ParticleShapeScale(transform.localScale);
        transform.position = owner.position + Vector3.up;
        transform.rotation = owner.rotation;
        direction = owner.forward.normalized;
        condition = owner.GetComponent<Condition>();
        targetLayer = enemyLayerMask;
    }

    public void Launch()
    {
        if (owner == null) return;
        // TODO : 발사
        float waitTime = projectileData.waitTime / condition.CurrentStat.attackSpeed;
        Invoke(nameof(ApplyLaunch), waitTime);
    }

    private void ApplyLaunch()
    {
        EnableHitBox(true);
        timer = Time.time;
        isLaunched = true;
    }

    private void Move()
    {
        if (projectileData.moveSpeed > 0)
        {
            transform.position += direction * projectileData.moveSpeed * Time.deltaTime;
        }
    }

    private void DestroySelf()
    {
        transform.DOKill();
        Managers.Pool.Despawn(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO : 충돌 처리
        if (Utils.LayerMaskContains(targetLayer, other.gameObject.layer))
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                Condition condition = owner.GetComponent<Condition>();
                float damage = condition.CurrentStat.attack * (projectileData.damagePer / 100);
                damageable.TakeDamage(damage);
                DestroySelf();
            }
        }
    }
}