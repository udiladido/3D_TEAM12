using System;

[Serializable]
public struct CombatData
{
    public int comboIndex; // 몇번째 콤보인지
    public float manaCost; // 소모 마나
    public float cooltime; // 쿨타임
    public float duration; // 지속시간
    public float startScale; // 시작 크기
    public float endScale; // 종료 크기
    public float moveSpeed; // 이동속도
    public float damagePer; // 데미지(본체 공격력 퍼센트)
    public float waitTime; // x초 후 발사(애니메이션 타이밍 맞추기용)
    public float knockbackPower; // 넉백 파워
    public float hitInterval;
    public int numberOfProjectilePerShot; // 다중 발사 수
    public float multipleProjectilesAngle; // 다중 발사 각도
    public int pierceCount; // 관통 수 (0이면 무한)
    
    public CombatData(ItemWeaponCombatEntity entity)
    {
        this.comboIndex = entity.comboIndex;
        this.manaCost = entity.manaCost;
        this.cooltime = entity.cooltime;
        this.duration = entity.duration;
        this.startScale = entity.startScale;
        this.endScale = entity.endScale;
        this.moveSpeed = entity.moveSpeed;
        this.damagePer = entity.damagePer;
        this.waitTime = entity.waitTime;
        this.knockbackPower = entity.knockbackPower;
        this.hitInterval = entity.hitInterval;
        this.numberOfProjectilePerShot = entity.numberOfProjectilePerShot;
        this.multipleProjectilesAngle = entity.multipleProjectilesAngle;
        this.pierceCount = entity.pierceCount;
    }
}