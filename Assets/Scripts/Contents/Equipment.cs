using System;
using UnityEngine;

public class Equipment : MonoBehaviour, IEquipment
{
    public event Action<Defines.UIEquipmentType, ItemEntity> OnEquipped;
    
    public ItemEntity EquippedComboWeapon { get; private set; }
    public StatData ComboWeaponStatData { get; private set; }
    public ItemEntity EquippedWeapon { get; private set; }
    public StatData WeaponStatData { get; private set; }

    public ItemEntity EquippedArmor { get; private set; }
    public StatData ArmorStatData { get; private set; }
    public ItemEntity EquippedAccessory { get; private set; }
    public StatData AccessoryStatData { get; private set; }

    private ModelPivot modelPivot;
    private CombatSlots combatSlots;
    private IStatHandler statHandler;

    public void LoadModel(Transform modelTransform)
    {
        UnLoadModel();
        modelPivot = modelTransform.GetComponent<ModelPivot>();
        combatSlots = GetComponent<CombatSlots>();
        statHandler = GetComponent<IStatHandler>();
        Equip(EquippedComboWeapon);
        Equip(EquippedWeapon);
    }

    public void UnLoadModel()
    {
        if (modelPivot == null) return;
        Managers.Resource.Destroy(modelPivot.gameObject);
        modelPivot = null;
    }

    public void Equip(ItemEntity item)
    {
        if (item == null || item.itemType != Defines.ItemType.Equipment || item.equipableEntity == null) return;
        if (item.equipableEntity.equipmentType == Defines.ItemEquipmentType.Weapon)
        {
            if (item.equipableEntity.combatStyleType == Defines.CharacterCombatStyleType.ComboAttack)
            {
                UnEquip(EquippedComboWeapon);
                EquippedComboWeapon = item;
                modelPivot?.EquipRightHand(item.equipableEntity);
                combatSlots.AddSlot(Defines.CharacterAttackInputType.ComboAttack, item.equipableEntity);
                ComboWeaponStatData = new StatData(item.statBoostEffectEntities);
                statHandler?.AddStatModifier(ComboWeaponStatData.statModifiers);
                OnEquipped?.Invoke(Defines.UIEquipmentType.ComboWeapon, item);
            }
            else
            {
                UnEquip(EquippedWeapon);
                EquippedWeapon = item;
                modelPivot?.EquipLeftHand(item.equipableEntity);
                combatSlots.AddSlot(Defines.CharacterAttackInputType.Skill, item.equipableEntity);
                WeaponStatData = new StatData(item.statBoostEffectEntities);
                statHandler?.AddStatModifier(WeaponStatData.statModifiers);
                OnEquipped?.Invoke(Defines.UIEquipmentType.SkillWeapon, item);
            }
        }
        else
        {
            if (item.equipableEntity.equipmentType == Defines.ItemEquipmentType.Armor)
            {
                UnEquip(EquippedArmor);
                EquippedArmor = item;
                ArmorStatData = new StatData(item.statBoostEffectEntities);
                statHandler?.AddStatModifier(ArmorStatData.statModifiers);
                OnEquipped?.Invoke(Defines.UIEquipmentType.Armor, item);
            }
            else if (item.equipableEntity.equipmentType == Defines.ItemEquipmentType.Accessory)
            {
                UnEquip(EquippedAccessory);
                EquippedAccessory = item;
                AccessoryStatData = new StatData(item.statBoostEffectEntities);
                statHandler?.AddStatModifier(AccessoryStatData.statModifiers);
                OnEquipped?.Invoke(Defines.UIEquipmentType.Accessory, item);
            }
        }
    }
    public void UnEquip(ItemEntity item)
    {
        if (item == null || item.itemType != Defines.ItemType.Equipment || item.equipableEntity == null) return;
        if (item.equipableEntity.equipmentType == Defines.ItemEquipmentType.Weapon)
        {
            if (item.equipableEntity.combatStyleType == Defines.CharacterCombatStyleType.ComboAttack)
            {
                EquippedComboWeapon = null;
                modelPivot?.UnEquipLeftHand();
                combatSlots.RemoveSlot(Defines.CharacterAttackInputType.ComboAttack);
                statHandler?.RemoveStatModifier(ComboWeaponStatData.statModifiers);
                ComboWeaponStatData = null;
                OnEquipped?.Invoke(Defines.UIEquipmentType.ComboWeapon, item);
            }
            else
            {
                EquippedWeapon = null;
                modelPivot?.UnEquipLeftHand();
                combatSlots.RemoveSlot(Defines.CharacterAttackInputType.Skill);
                statHandler?.RemoveStatModifier(WeaponStatData.statModifiers);
                WeaponStatData = null;
                OnEquipped?.Invoke(Defines.UIEquipmentType.SkillWeapon, item);
            }
        }
        else
        {
            if (item.equipableEntity.equipmentType == Defines.ItemEquipmentType.Armor)
            {
                EquippedArmor = null;
                statHandler?.RemoveStatModifier(ArmorStatData.statModifiers);
                ArmorStatData = null;
                OnEquipped?.Invoke(Defines.UIEquipmentType.Armor, item);
            }
            else if (item.equipableEntity.equipmentType == Defines.ItemEquipmentType.Accessory)
            {
                EquippedAccessory = null;
                statHandler?.RemoveStatModifier(AccessoryStatData.statModifiers);
                AccessoryStatData = null;
                OnEquipped?.Invoke(Defines.UIEquipmentType.Accessory, item);
            }
        }
    }
    public void UnEquip(Defines.ItemEquipmentType equipmentType,
        Defines.CharacterCombatStyleType combatStyleType = Defines.CharacterCombatStyleType.None)
    {
        switch (equipmentType)
        {
            case Defines.ItemEquipmentType.Weapon:
                UnEquip(combatStyleType == Defines.CharacterCombatStyleType.ComboAttack ? EquippedComboWeapon : EquippedWeapon);
                break;
            case Defines.ItemEquipmentType.Armor:
                UnEquip(EquippedArmor);
                break;
            case Defines.ItemEquipmentType.Accessory:
                UnEquip(EquippedAccessory);
                break;
        }
    }
    
    public bool CanAction(Defines.CharacterAttackInputType inputType)
    {
        if (inputType == Defines.CharacterAttackInputType.Skill)
            return EquippedWeapon != null;
        else if (inputType == Defines.CharacterAttackInputType.ComboAttack)
            return EquippedComboWeapon != null;

        return false;
    }

}