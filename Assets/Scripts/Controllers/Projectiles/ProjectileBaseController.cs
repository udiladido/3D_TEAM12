using UnityEngine;

public abstract class ProjectileBaseController : BaseController
{

    protected ParticleSystem parentParticleSystem;
    protected ParticleSystem[] particleSystems;
    protected MeshRenderer meshRenderer;
    protected Collider hitBoxCollider;
    
    
    protected virtual void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        hitBoxCollider = GetComponentInChildren<Collider>();
        parentParticleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }
    
    public abstract void Launch();
    protected abstract void ApplyLaunch();
    protected virtual void DestroySelf()
    {
        Managers.Pool.Despawn(gameObject);
    }
    public virtual void SetData(Transform owner, CombatData projectileData, LayerMask enemyLayerMask)
    {
        
    }
    public virtual void SetData(Transform owner, CombatData projectileData, LayerMask enemyLayerMask, Vector3 direction)
    {
        
    }
    public virtual void SetData(Transform owner, Transform target, MonsterSkillEntity skillEntity, LayerMask targetLayer)
    {
        
    }

    protected void EnableHitBox(bool enable)
    {
        if (hitBoxCollider != null)
            hitBoxCollider.enabled = enable;
        if (meshRenderer != null)
            meshRenderer.enabled = enable;
        if (parentParticleSystem != null)
            parentParticleSystem.gameObject.SetActive(enable);
    }
    
}