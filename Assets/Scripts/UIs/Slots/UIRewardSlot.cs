using System.Text;
using UnityEngine;

public class UIRewardSlot : UISlotBase
{
    enum Images { Background, Icon }

    enum Texts { Title, Description, StatNames, StatValues }

    enum Buttons { RewardButton }

    private ItemEntity item;
    private UIRewardPopup popup;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton(Buttons.RewardButton).gameObject.BindEvent(() => { popup?.SelectedItem(item); });

        return true;
    }

    public void SetParent(UIRewardPopup popup)
    {
        this.popup = popup;
    }

    public void SetData(ItemEntity item, int count = 1)
    {
        this.item = item;
        GetText(Texts.Title).text = $"{item.displayTitle}{(count > 1 ? $" x{count}" : "")}";
        GetText(Texts.Description).text = item.description;
        GetImage(Images.Icon).sprite = Managers.Resource.Load<Sprite>(item.iconPath);
        GetImage(Images.Background).color = Utils.GetItemRarityColor(item.rarityType);
        GetText(Texts.StatNames).text = string.Empty;
        GetText(Texts.StatValues).text = string.Empty;
        if (item.statBoostEffectEntities != null)
        {
            StringBuilder statNames = new StringBuilder();
            StringBuilder statValues = new StringBuilder();
            foreach (var effect in item.statBoostEffectEntities)
            {
                statNames.AppendLine(Utils.GetStatName(effect.statType));
                statValues.AppendLine($"+{effect.amount}");
            }
            
            GetText(Texts.StatNames).text = statNames.ToString();
            GetText(Texts.StatValues).text = statValues.ToString();
        }
    }
}