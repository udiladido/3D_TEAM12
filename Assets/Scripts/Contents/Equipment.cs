using UnityEngine;

public class Equipment : MonoBehaviour, IEquipable
{
    public ItemEntity EquippedComboWeapon { get; private set; }
    public ItemEntity EquippedWeapon { get; private set; }

    private ModelPivot modelPivot;
    private void Awake()
    {
        modelPivot = GetComponentInChildren<ModelPivot>();
    }

    public void Equip(ItemEntity item)
    {
        if (item == null) return;
        if (item.equipableEntity.combatStyleType == Defines.CharacterCombatStyleType.ComboAttack)
        {
            UnEquip(EquippedComboWeapon);
            EquippedComboWeapon = item;
            modelPivot?.EquipLeftHand(item.equipableEntity);
        }
        else
        {
            UnEquip(EquippedWeapon);
            EquippedWeapon = item;
            modelPivot?.EquipLeftHand(item.equipableEntity);
        }
        
    }
    public void UnEquip(ItemEntity item)
    {
        if (item == null) return;
        if (item.equipableEntity.combatStyleType == Defines.CharacterCombatStyleType.ComboAttack)
        {
            EquippedComboWeapon = null;
            modelPivot?.UnEquipLeftHand();
        }
        else
        {
            EquippedWeapon = null;
            modelPivot?.UnEquipLeftHand();
        }
    }
    public ItemEquipableEntity GetWeaponInfo(Defines.CharacterAttackInputType inputType)
    {
        if (inputType == Defines.CharacterAttackInputType.Attack)
            return EquippedWeapon?.equipableEntity;
        else if (inputType == Defines.CharacterAttackInputType.ComboAttack)
            return EquippedComboWeapon?.equipableEntity;

        return null;
    }
}