using System;
using UnityEngine;

public class QuickSlotItem
{
    public ItemQuickSlots itemQuickSlots { get; private set; }
    public Defines.ItemQuickSlotInputType inputType { get; private set; }

    public ItemEntity itemEntity { get; private set; }
    
    public int Count { get; private set; }

    public QuickSlotItem(ItemQuickSlots itemQuickSlots, Defines.ItemQuickSlotInputType inputType)
    {
        this.itemQuickSlots = itemQuickSlots;
        this.inputType = inputType;
    }

    public void SetData(ItemEntity itemEntity)
    {
        this.itemEntity = itemEntity;
    }
    
    public void AddCount(int count)
    {
        Count += count;
    }


    public void Use()
    {
        if (itemEntity == null) return;

        if (itemEntity.itemType == Defines.ItemType.Consumable)
        {
            foreach (var consume in itemEntity.consumableEntities)
            {
                float amount = Utils.Operation(consume.calcType).Invoke(itemQuickSlots.Condition.CurrentStat.maxHp, consume.amount);
                switch (consume.consumableType)
                {
                    case Defines.ItemConsumableType.HpRecovery:
                        itemQuickSlots.Condition.Heal(amount);
                        break;
                    case Defines.ItemConsumableType.MpRecovery:
                        itemQuickSlots.Condition.MpRecovery(amount);
                        break;
                }
            }
            
            Count--;
            if (Count <= 0)
                itemQuickSlots.UnEquip(inputType);
        }
    }
}