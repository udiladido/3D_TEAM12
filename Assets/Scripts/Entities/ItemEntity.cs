using System;
using System.Collections.Generic;

[Serializable]
public class ItemEntity : EntityBase
{
    public Defines.ItemType itemType;
    public Defines.ItemRarityType rarityType;
    public string dropPrefabPath;
    public string iconPath;
    
    public ItemEquipableEntity equipableEntity;
    public List<ItemStatBoostEffectEntity> statBoostEffectEntities;
    public List<ItemConsumableEntity> consumableEntities;
}

[Serializable]
public class ItemShopInfoEntity
{
    public int itemId;
    public long price;
}

[Serializable]
public class ItemEquipableEntity
{
    public int itemId;
    public Defines.ItemEquipmentType equipmentType;
    public string equipedPrefabPath;
    public Defines.CharacterCombatStyleType combatStyleType;
    
    public List<ItemWeaponCombatEntity> weaponCombatEntities;
}

[Serializable]
public class ItemWeaponCombatEntity
{
    public int itemId;
    public int comboIndex; // 몇번째 콤보인지
    public string projectilePrefabPath; // 공격 프로젝타일 프리팹 경로
    public float manaCost; // 소모 마나
    public float cooltime; // 쿨타임
    public float duration; // 지속시간
    public float startScale; // 시작 크기
    public float endScale; // 종료 크기
    public float moveSpeed; // 이동속도
    public float damagePer; // 데미지(본체 공격력 퍼센트)
    public float waitTime; // x초 후 발사(애니메이션 타이밍 맞추기용)
    public float knockbackPower; // 넉백 파워
    public float hitInterval; // 피격 간격
}

[Serializable]
public class ItemStatBoostEffectEntity
{
    public int itemId;
    public Defines.CharacterStatType statType;
    public int amount;
    public Defines.CalcType calcType;
}

[Serializable]
public class ItemConsumableEntity
{
    public int itemId;
    public Defines.ItemConsumableType consumableType;
    public float amount;
    public float duration;
    public Defines.CalcType calcType;
}