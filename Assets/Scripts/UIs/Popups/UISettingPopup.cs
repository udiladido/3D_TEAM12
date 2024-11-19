using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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




    protected override bool Init()
    {



        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindToggle(typeof(Toggle));
        BindSlider(typeof(Slider));

        GetButton(Buttons.CloseButton).gameObject.BindEvent(() => { Close(Defines.UIAnimationType.Bounce); });
        GetToggle(Toggle.MuteToggle).gameObject.BindEvent(OnMuteClick);

        GetSlider(Slider.BGMSlider).onValueChanged.AddListener(SetSFXVolume);
        GetSlider(Slider.SFXSlider).onValueChanged.AddListener(SetBGMVolume);

        return true;
    }



    public void SetSFXVolume(float value)
    {
        if(Managers.Sound.IsSoundOn)
        Managers.Sound.SetSFXVolume(value);
        else
            Managers.Sound.SetSFXVolume(0);

    }

    public void SetBGMVolume(float value)
    {
        if (Managers.Sound.IsSoundOn)
            Managers.Sound.SetSFXVolume(value);
        else
            Managers.Sound.SetSFXVolume(0);

    }


    // 음소거 이벤트
    public void OnMuteClick()
    {

        //  토글이 체크 되어 있을 때 = 음소거 해제
        if (Managers.Sound.IsSoundOn)
        {
         
            Managers.Sound.PrevSoundBgmValue = GetSlider(Slider.BGMSlider).value;
            Managers.Sound.PrevSoundSfxValue = GetSlider(Slider.SFXSlider).value;

            GetSlider(Slider.BGMSlider).value = 0f;
            GetSlider(Slider.SFXSlider).value = 0f;

            Managers.Sound.IsSoundOn = false;
        }

        // 토글이 체크 해제 되어 있을 때 = 음소거
        else 
        {

            GetSlider(Slider.BGMSlider).value = Managers.Sound.PrevSoundBgmValue;
            GetSlider(Slider.SFXSlider).value = Managers.Sound.PrevSoundSfxValue;

            Managers.Sound.IsSoundOn = true;

        }





    }

}
