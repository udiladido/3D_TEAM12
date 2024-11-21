using UnityEngine;

public abstract class ProjectileBaseController : BaseController
{
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
    
}