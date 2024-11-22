using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIRewardPopup : UIPopupBase
{
    private int rewardCount = 3;

    enum Objects { Layout }

    private List<UIRewardSlot> slots;

    private Queue<ItemEntity> comboWeapons;
    private Queue<ItemEntity> armors;
    private Queue<ItemEntity> accessories;
    private List<ItemEntity> skillWeapons;

    private ItemEntity hpPotion;
    private ItemEntity mpPotion;
    
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind<GameObject>(typeof(Objects));
        slots = new List<UIRewardSlot>();
        GameObject layoutGo = GetObject(Objects.Layout);
        for (int i = 0; i < rewardCount; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Slot/UIRewardSlot", layoutGo.transform);
            UIRewardSlot slot = go.GetOrAddComponent<UIRewardSlot>();
            slot.SetParent(this);
            slots.Add(slot);
        }

        InitRewards();
        return true;
    }

    private void InitRewards()
    {

        comboWeapons = new Queue<ItemEntity>();
        skillWeapons = new List<ItemEntity>();
        armors = new Queue<ItemEntity>();
        accessories = new Queue<ItemEntity>();

        foreach (var item in Managers.DB.GetAll<ItemEntity>()
                     .OrderBy(s => s.rarityType))
        {
            if (item.id == Defines.DEFAULT_COMBO_WEAPON_ID) continue;
            if (item.itemType == Defines.ItemType.Consumable)
            {
                if (item.id == Defines.REWARD_HP_POTION_ID)
                    hpPotion = item;
                else if (item.id == Defines.REWARD_MP_POTION_ID)
                    mpPotion = item;
            }
            else if (item?.equipableEntity.equipmentType == Defines.ItemEquipmentType.Weapon)
            {
                if (item.equipableEntity?.combatStyleType == Defines.CharacterCombatStyleType.ComboAttack)
                {
                    comboWeapons.Enqueue(item);
                }
                else
                {
                    skillWeapons.Add(item);
                }
            }
            else if (item?.equipableEntity.equipmentType == Defines.ItemEquipmentType.Armor)
            {
                armors.Enqueue(item);
            }
            else if (item?.equipableEntity.equipmentType == Defines.ItemEquipmentType.Accessory)
            {
                accessories.Enqueue(item);
            }
        }
    }

    public void SelectedItem(ItemEntity item)
    {
        if (item.equipableEntity?.combatStyleType == Defines.CharacterCombatStyleType.ComboAttack)
        {
            comboWeapons.Dequeue();
        }
        else if (item.equipableEntity?.equipmentType == Defines.ItemEquipmentType.Armor)
        {
            armors.Dequeue();
        }
        else if (item.equipableEntity?.equipmentType == Defines.ItemEquipmentType.Accessory)
        {
            accessories.Dequeue();
        }

        if (item.itemType == Defines.ItemType.Equipment)
            Managers.Game.Player.Equipment.Equip(item);
        else if (item.itemType == Defines.ItemType.Consumable)
            Managers.Game.Player.ItemQuickSlots.Equip(item, 3);

        Managers.Game.Player.ItemQuickSlots.Equip(hpPotion);
        Managers.Game.Player.ItemQuickSlots.Equip(mpPotion);
        
        Managers.UI.ClosePopupUI<UIRewardPopup>();
    }
    

    public override void Open(Defines.UIAnimationType type = Defines.UIAnimationType.None)
    {
        base.Open(type);
        Managers.Game.StopGame();
        List<ItemEntity> items = GetRewards();

        for (int i = 0; i < rewardCount; i++)
            slots[i].SetData(items[i]);
    }

    public override void Close(Defines.UIAnimationType type = Defines.UIAnimationType.None)
    {
        Managers.Game.ResumeGame();
        Managers.Game.StartNextWave();
        base.Close(type);
    }


    private List<ItemEntity> GetRewards()
    {
        var equipment = Managers.Game.Player.Equipment;

        List<ItemEntity> items = new List<ItemEntity>();
        while (items.Count < rewardCount)
        {
            Defines.UIRewardItemType randomType = (Defines.UIRewardItemType)Random.Range(1, (int)Defines.UIRewardItemType.ItemCount);
            ItemEntity randomItem = null;
            switch (randomType)
            {
                case Defines.UIRewardItemType.ComboWeapon:
                    comboWeapons.TryPeek(out randomItem);
                    break;
                case Defines.UIRewardItemType.SkillWeapon:
                    randomItem = GetSkillWeapon(equipment.EquippedWeapon);
                    break;
                case Defines.UIRewardItemType.Armor:
                    armors.TryPeek(out randomItem);
                    break;
                case Defines.UIRewardItemType.Accessory:
                    accessories.TryPeek(out randomItem);
                    break;

            }

            if (randomItem == null)
                randomItem = GetRandomConsumeableItem();

            if (items.Contains(randomItem) == false)
                items.Add(randomItem);
        }

        return items;
    }

    private ItemEntity GetSkillWeapon(ItemEntity currentItem)
    {
        int randomIndex = Random.Range(0, skillWeapons.Count - 1);
        if (skillWeapons[randomIndex].id == currentItem?.id)
            return skillWeapons[(randomIndex + 1) % skillWeapons.Count];

        return skillWeapons[randomIndex];
    }

    private ItemEntity GetRandomConsumeableItem()
    {
        return Random.Range(0, 2) == 0 ? hpPotion : mpPotion;
    }

}