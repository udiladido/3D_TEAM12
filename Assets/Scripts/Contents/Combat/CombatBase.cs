using UnityEngine;

public abstract class CombatBase
{
    public ItemEquipableEntity EquipEntity { get; private set; }
    public Defines.CharacterAttackInputType AttackInput { get; private set; }
    protected CombatSlots combatSlots;
    protected AnimationData animationData;

    protected float cooltime;
    protected float maxCooltime;

    public abstract void Execute();

    public CombatBase(CombatSlots combatSlots)
    {
        this.combatSlots = combatSlots;
    }

    public virtual void Update()
    {

    }

    public virtual void SetData(Defines.CharacterAttackInputType attackInput, ItemEquipableEntity equipEntity)
    {
        this.AttackInput = attackInput;
        this.EquipEntity = equipEntity;
    }

    public void RemoveData()
    {
        this.EquipEntity = null;
    }

    public void Timer()
    {
        if (cooltime <= 0) return;
        cooltime -= Time.deltaTime;
        combatSlots.UpdateCooltime(AttackInput, cooltime, maxCooltime);
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