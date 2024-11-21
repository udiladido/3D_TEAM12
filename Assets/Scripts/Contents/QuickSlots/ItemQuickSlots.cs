using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemQuickSlots : MonoBehaviour
{
    public event Action<Defines.ItemQuickSlotInputType, QuickSlotItem> OnQuickSlotChanged;
    
    private Dictionary<Defines.ItemQuickSlotInputType, QuickSlotItem> quickSlots =
        new Dictionary<Defines.ItemQuickSlotInputType, QuickSlotItem>();
    public Condition Condition { get; private set; }
    private void Start()
    {
        Condition = GetComponent<Condition>();
        InitQuickSlots();
    }

    public void InitQuickSlots()
    {
        quickSlots.Clear();
        Defines.ItemQuickSlotInputType[] quickSlotInputTypes =
            (Defines.ItemQuickSlotInputType[])System.Enum.GetValues(typeof(Defines.ItemQuickSlotInputType));
        foreach (var type in quickSlotInputTypes)
        {
            if (type == Defines.ItemQuickSlotInputType.None) continue;
            quickSlots.Add(type, null);
        }
    }


    public void Equip(ItemEntity itemEntity, int count = 1)
    {
        Defines.ItemQuickSlotInputType slotInputType = GetSlot(itemEntity.id);
        if (slotInputType == Defines.ItemQuickSlotInputType.None)
            slotInputType = GetEmptySlot();
        if (slotInputType == Defines.ItemQuickSlotInputType.None) return;
        if (quickSlots.ContainsKey(slotInputType) && itemEntity.itemType == Defines.ItemType.Consumable)
        {
            if (quickSlots[slotInputType] == null)
            {
                QuickSlotItem quickSlotItem = new QuickSlotItem(this, slotInputType);
                quickSlotItem.SetData(itemEntity);
                quickSlots[slotInputType] = quickSlotItem;
            }

            quickSlots[slotInputType].AddCount(count);
            OnQuickSlotChanged?.Invoke(slotInputType, quickSlots[slotInputType]);
        }
    }

    public void UnEquip(Defines.ItemQuickSlotInputType inputType)
    {
        if (inputType == Defines.ItemQuickSlotInputType.None) return;
        if (quickSlots.ContainsKey(inputType))
        {
            quickSlots[inputType] = null;
            OnQuickSlotChanged?.Invoke(inputType, null);
        }
    }

    public void Use(Defines.ItemQuickSlotInputType inputType)
    {
        if (quickSlots.TryGetValue(inputType, out QuickSlotItem item))
        {
            item?.Use();
            OnQuickSlotChanged?.Invoke(inputType, item);
        }
    }

    private Defines.ItemQuickSlotInputType GetEmptySlot()
    {
        foreach (var slot in quickSlots)
        {
            if (slot.Value == null)
                return slot.Key;
        }

        return Defines.ItemQuickSlotInputType.None;
    }

    private Defines.ItemQuickSlotInputType GetSlot(int itemId)
    {
        foreach (var slot in quickSlots)
        {
            if (slot.Value != null && slot.Value.itemEntity?.id == itemId)
                return slot.Key;
        }

        return Defines.ItemQuickSlotInputType.None;
    }
    
    public QuickSlotItem GetQuickSlotItem(Defines.ItemQuickSlotInputType inputType)
    {
        return quickSlots.GetValueOrDefault(inputType);
    }
}