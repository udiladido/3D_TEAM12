using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIStatusBarSlot : UISlotBase
{
    enum Images { SliderImage, OverrayMask }
    
    public Defines.UIStatusType statusType;
    
    private Image sliderImage;
    private Image OverrayImage;
    private Vector3 initialPosition;
    private bool isShaking;
    
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
        OverrayImage = GetImage(Images.OverrayMask);
        Condition condition = Managers.Game.Player.Condition;
        if (condition == null) return;
        if (statusType == Defines.UIStatusType.Hp)
        {
            Managers.Game.Player.Condition.OnHpChanged += SetValue;
            Managers.Game.Player.Condition.OnHpWarning += ShakeMotion;
        }
        else if (statusType == Defines.UIStatusType.Mp)
        {
            Managers.Game.Player.Condition.OnMpChanged += SetValue;
            Managers.Game.Player.Condition.OnMpWarning += ShakeMotion;
        }
        initialPosition = this.transform.localPosition;
    }

    public void SetValue(float value, float maxValue)
    {
        sliderImage.fillAmount = value / maxValue;
    }
    
    public void ShakeMotion()
    {
        if (isShaking == false)
        {
            isShaking = true;
            this.transform.DOShakePosition(duration: 0.5f, strength: 10f, 20, 50f, true, true)
                .OnComplete(ShakeCompele);
            if (statusType == Defines.UIStatusType.Hp) OverrayImage.color = new Color(1, 0, 0, 1);
            else OverrayImage.color = new Color(0, 0, 1, 1);
            OverrayImage.DOFade(0f, 0.5f);
        } 
    }
    private void ShakeCompele()
    {
        this.transform.localPosition = initialPosition;
        isShaking = false;
    }
}