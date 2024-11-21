using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDodgeSlot : UISlotBase
{
    enum Images { EquippedIcon, CooltimeSlider, BackGround }

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
        Managers.Game.Player.Condition.OnDodgeTimerChanged -= SetCooltime;
        Managers.Game.Player.Condition.OnDodgeTimerChanged += SetCooltime;
        
    }

    private void SetCooltime(float cooltime, float maxCooltime)
    {
        float amount = (maxCooltime - cooltime) / maxCooltime;
        
        if (amount > 0 && cooltimeSlider.gameObject.activeInHierarchy == false)
            cooltimeSlider.gameObject.SetActive(true);
        else if (amount <= 0 && cooltimeSlider.gameObject.activeInHierarchy)
            cooltimeSlider.gameObject.SetActive(false);
        if (cooltimeSlider.gameObject.activeInHierarchy)
            cooltimeSlider.fillAmount = amount;
    }
}