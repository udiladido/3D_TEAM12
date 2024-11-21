using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuickSlot : UISlotBase
{
    enum Images { EquippedIcon, BackGround }
    enum Texts { ItemCount, QuickSlotKey }

    public Defines.ItemQuickSlotInputType quickSlotType;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        return true;
    }

    private void Start()
    {
        GetText(Texts.QuickSlotKey).text = ((int)quickSlotType).ToString();
        Managers.Game.Player.ItemQuickSlots.OnQuickSlotChanged -= QuickSlotChanged;
        Managers.Game.Player.ItemQuickSlots.OnQuickSlotChanged += QuickSlotChanged;
        ItemQuickSlots quickSlots = Managers.Game.Player.ItemQuickSlots;
        QuickSlotChanged(quickSlotType, quickSlots.GetQuickSlotItem(quickSlotType));
    }

    private void QuickSlotChanged(Defines.ItemQuickSlotInputType slotInputType, QuickSlotItem item)
    {
        if (slotInputType == quickSlotType)
        {
            Image icon = GetImage(Images.EquippedIcon);
            Image background = GetImage(Images.BackGround);
            TMP_Text countText = GetText(Texts.ItemCount);
            if (item == null)
            {
                icon.sprite = null;
                background.color = Utils.GetItemRarityColor(Defines.ItemRarityType.None);
                countText.text = "0";
            }
            else
            {
                icon.sprite = Managers.Resource.Load<Sprite>(item.itemEntity.iconPath, true);
                countText.text = item.Count.ToString();
                background.color = Utils.GetItemRarityColor(item.itemEntity.rarityType);
            }
        }
    }
    
}