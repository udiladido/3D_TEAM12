using UnityEngine;

public class Equipment : MonoBehaviour, IEquipable
{
    public ItemEntity EquippedComboWeapon { get; private set; }
    public ItemEntity EquippedWeapon { get; private set; }

    private ModelPivot modelPivot;
    private void Awake()
    {
        LoadModel();
    }

    public void LoadModel()
    {
        UnLoadModel();
        modelPivot = GetComponentInChildren<ModelPivot>();
    }

    public void UnLoadModel()
    {
        if (modelPivot == null) return;
        Managers.Resource.Destroy(modelPivot.gameObject);
        modelPivot = null;
    }

    public void Equip(ItemEntity item)
    {
        if (item == null) return;
        if (item.equipableEntity.combatStyleType == Defines.CharacterCombatStyleType.ComboAttack)
        {
            UnEquip(EquippedComboWeapon);
            EquippedComboWeapon = item;
            modelPivot?.EquipRightHand(item.equipableEntity);
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
        if (inputType == Defines.CharacterAttackInputType.Skill)
            return EquippedWeapon?.equipableEntity;
        else if (inputType == Defines.CharacterAttackInputType.ComboAttack)
            return EquippedComboWeapon?.equipableEntity;

        return null;
    }
}