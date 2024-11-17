public interface IEquipable
{
    void Equip(ItemEntity item);
    void UnEquip(ItemEntity item);

    ItemEquipableEntity GetWeaponInfo(Defines.CharacterAttackInputType inputType);
}