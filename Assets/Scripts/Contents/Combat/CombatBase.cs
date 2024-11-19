using UnityEngine;

public abstract class CombatBase
{
    public ItemEquipableEntity EquipEntity { get; private set; }
    protected CombatSlots combatSlots;
    protected AnimationData animationData;

    protected float cooltime;

    public abstract void Use();

    public CombatBase(CombatSlots combatSlots)
    {
        this.combatSlots = combatSlots;
        combatSlots.OnTimerChanged += Timer;
    }

    public virtual void Update()
    {
        
    }

    public virtual void SetData(ItemEquipableEntity equipEntity)
    {
        this.EquipEntity = equipEntity;
    }

    public void RemoveData()
    {
        this.EquipEntity = null;
    }

    private void Timer()
    {
        if (cooltime <= 0) return;
        cooltime -= Time.deltaTime;
    }
    
    protected virtual void ApplyAttack(string projectilePrefabPath, CombatData combatData, Transform parent = null)
    {
        GameObject go = Managers.Pool.Spawn(projectilePrefabPath, parent);
        if (go == null) return;
        ProjectileController projectile = go.GetComponent<ProjectileController>();
        projectile.SetData(combatSlots.transform, combatData, combatSlots.EnemyLayerMask);
        projectile.Launch();
    }
}