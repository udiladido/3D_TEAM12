public interface IEquipment
{
    void Equip(ItemEntity item);
    void UnEquip(ItemEntity item);
    
    bool CanAction(Defines.CharacterAttackInputType inputType);

    ItemEquipableEntity GetWeaponInfo(Defines.CharacterAttackInputType inputType);
}