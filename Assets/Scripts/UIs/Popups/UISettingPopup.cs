using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingPopup : UIPopupBase
{

    enum Buttons
    {
        CloseButton,

    }

    enum Toggle
    { 
        MuteToggle,
    
    }

    enum Slider
    {
        BGMSlider,
        SFXSlider

    }


    // 고민해봐야 할 부분 - 씬 넘어갔다 와도 저장되어 있어야 하는데
    private bool isOn;
    private float prevSoundValue;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindSlider(typeof(Slider));

        GetButton(Buttons.CloseButton).gameObject.BindEvent(() => { Close(Defines.UIAnimationType.Bounce); });
        GetToggle(Toggle.MuteToggle).gameObject.BindEvent(OnMuteClick);
        GetSlider(Slider.BGMSlider).gameObject.BindEvent();
        GetSlider(Slider.SFXSlider).gameObject.BindEvent();

        return true;
    }



    public void SetSFXVolume(float sliderValue)
    {





    }

    public void SetBGMVolume(float sliderValue)
    {


    }


    // 음소거 이벤트
    public void OnMuteClick()
    {

        //  토글이 체크 되어 있을 때
        if (isOn){ 
        

        }

        // 토글이 체크 해제 되어 있을 때
        else { 
        
        }





    }

}
