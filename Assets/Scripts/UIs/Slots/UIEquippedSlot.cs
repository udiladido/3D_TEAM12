using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEquippedSlot : UISlotBase
{
    enum Images { EquippedIcon, CooltimeSlider, BackGround }


    public Defines.UIEquipmentType equipmentType;

    private Image cooltimeSlider;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));

        return true;
    }

    private void Start()
    {
        cooltimeSlider = GetImage(Images.CooltimeSlider);
        cooltimeSlider.gameObject.SetActive(false);
        Managers.Game.Player.Equipment.OnEquipped -= SetEquipped;
        Managers.Game.Player.Equipment.OnEquipped += SetEquipped;
        Equipment equipment = Managers.Game.Player.Equipment;
        if (equipmentType == Defines.UIEquipmentType.ComboWeapon)
        {
            Managers.Game.Player.CombatSlots.OnComboAttackCooltimeChanged -= SetSkillCooltime;
            Managers.Game.Player.CombatSlots.OnComboAttackCooltimeChanged += SetSkillCooltime;
            if (equipment != null)
                SetEquipped(equipmentType, equipment.EquippedComboWeapon);
        }
        else if (equipmentType == Defines.UIEquipmentType.SkillWeapon)
        {
            Managers.Game.Player.CombatSlots.OnSkillCooltimeChanged -= SetSkillCooltime;
            Managers.Game.Player.CombatSlots.OnSkillCooltimeChanged += SetSkillCooltime;
            if (equipment != null)
                SetEquipped(equipmentType, equipment.EquippedWeapon);
        }
        else if (equipmentType == Defines.UIEquipmentType.Armor)
        {
            if (equipment != null)
                SetEquipped(equipmentType, equipment.EquippedArmor);
        }
        else if (equipmentType == Defines.UIEquipmentType.Accessory)
        {
            if (equipment != null)
                SetEquipped(equipmentType, equipment.EquippedAccessory);
        }
            
    }

    private void SetEquipped(Defines.UIEquipmentType equipmentType, ItemEntity equip)
    {
        Image image = GetImage(Images.EquippedIcon);
        Image background = GetImage(Images.BackGround);
        if (this.equipmentType == equipmentType && equip != null)
        {
            image.sprite = Managers.Resource.Load<Sprite>(equip.iconPath);
            background.color = Utils.GetItemRarityColor(equip.rarityType);
        }
        else if (equip == null)
        {
            image.sprite = null;
            background.color = Utils.GetItemRarityColor(Defines.ItemRarityType.None);
        }
    }

    private void SetSkillCooltime(float cooltime, float maxCooltime)
    {
        if (cooltime > 0 && cooltimeSlider.gameObject.activeInHierarchy == false)
            cooltimeSlider.gameObject.SetActive(true);
        else if (cooltime <= 0 && cooltimeSlider.gameObject.activeInHierarchy)
            cooltimeSlider.gameObject.SetActive(false);
        if (cooltimeSlider.gameObject.activeInHierarchy)
            cooltimeSlider.fillAmount = cooltime / maxCooltime;
    }
}