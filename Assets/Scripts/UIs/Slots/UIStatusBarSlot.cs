using UnityEngine.UI;

public class UIStatusBarSlot : UISlotBase
{
    enum Images { SliderImage }
    
    public Defines.UIStatusType statusType;
    
    private Image sliderImage;
    
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        return true;
    }

    private void Start()
    {
        sliderImage = GetImage(Images.SliderImage);
        Condition condition = Managers.Game.Player.Condition;
        if (condition == null) return;
        if (statusType == Defines.UIStatusType.Hp)
        {
            Managers.Game.Player.Condition.OnHpChanged += SetValue;
        }
        else if (statusType == Defines.UIStatusType.Mp)
        {
            Managers.Game.Player.Condition.OnMpChanged += SetValue;
        }
    }

    public void SetValue(float value, float maxValue)
    {
        sliderImage.fillAmount = value / maxValue;
    }
    
    
}