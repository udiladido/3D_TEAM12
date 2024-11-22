using UnityEngine;

public abstract class ProjectileBaseController : BaseController
{

    protected ParticleSystem parentParticleSystem;
    protected ParticleSystem[] particleSystems;
    protected MeshRenderer meshRenderer;
    protected Collider hitBoxCollider;
    protected int hitCounter = 0;
    
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

    protected void Scale(float startScale, float endScale, float duration)
    {
        float scale = endScale - startScale;
        if (scale == 0) return;
        Vector3 scaleVector = Vector3.one * scale * Time.deltaTime / duration;
        transform.localScale += scaleVector;
        ParticleShapeScale(transform.localScale);
    }
    
    protected void ParticleShapeScale(Vector3 scaleVector)
    {
        foreach (var particle in particleSystems)
        {
            if (particle.main.scalingMode == ParticleSystemScalingMode.Hierarchy)
                continue;
            ParticleSystem.ShapeModule shape = particle.shape;
            shape.scale = scaleVector;
        }
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