using UnityEngine;

public class UIRewardSlot : UISlotBase
{
    enum Images { Background, Icon }
    enum Texts { Title, Description }
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

    public void SetData(ItemEntity item)
    {
        this.item = item;
        GetText(Texts.Title).text = item.displayTitle;
        GetText(Texts.Description).text = item.description;
        GetImage(Images.Icon).sprite = Managers.Resource.Load<Sprite>(item.iconPath);
        GetImage(Images.Background).color = Utils.GetItemRarityColor(item.rarityType);
    }
}