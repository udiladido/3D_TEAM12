using DG.Tweening;
using System;
using UnityEngine;

public class ProjectileController : BaseController
{
    private ItemWeaponCombatEntity data;
    private Transform owner;

    private Vector3 direction;

    private MeshRenderer meshRenderer;
    private Collider hitBoxCollider;

    private bool isLaunched;
    private Combat combat;
    private Condition condition; 

    private float timer;
    [SerializeField] private LayerMask targetLayer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        hitBoxCollider = GetComponentInChildren<Collider>();
    }

    private void Update()
    {
        if (isLaunched)
        {
            if (Time.time - timer > data.duration)
                DestroySelf();

            Move();
            Scale();

        }
    }

    private void Scale()
    {
        float scale = data.endScale - data.startScale;
        if (scale == 0) return;
        
        transform.localScale += Vector3.one * scale * Time.deltaTime / data.duration;
    }

    public void SetData(Transform owner, ItemWeaponCombatEntity data)
    {
        this.data = data;
        this.owner = owner;
        isLaunched = false;
        meshRenderer.enabled = false;
        hitBoxCollider.enabled = false;
        
        transform.localScale = new Vector3(data.startScale, data.startScale, data.startScale);
        transform.position = owner.position;
        transform.rotation = owner.rotation;
        direction = owner.forward.normalized;
        combat = owner.GetComponent<Combat>();
        condition = owner.GetComponent<Condition>();
        targetLayer = combat.EnemyLayerMask;
    }

    public void Launch()
    {
        if (owner == null) return;
        if (data == null) return;
        // TODO : 발사
        float waitTime = Defines.ATTACK_ANIMATION_SPEED_OFFSET / condition.CurrentStat.attackSpeed;
        Invoke(nameof(ApplyLaunch), waitTime);
    }

    private void ApplyLaunch()
    {
        meshRenderer.enabled = true;
        hitBoxCollider.enabled = true;
        timer = Time.time;
        isLaunched = true;
    }

    private void Move()
    {
        if (data.moveSpeed == 0)
        {
            transform.position = owner.position;
        }
        else
        {
            transform.position += direction * data.moveSpeed * Time.deltaTime;
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
                float damage = condition.CurrentStat.attack * (data.damagePer / 100);
                damageable.TakeDamage(damage);
                DestroySelf();
            }
        }
    }
}