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
        ExitButton,
    }

    enum Toggle
    { 
        MuteToggle,
    
    }

    enum Slider
    {
        BGMSlider,
        SFXSlider,
        MasterSlider

    }


    protected override bool Init()
    {

        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindToggle(typeof(Toggle));
        BindSlider(typeof(Slider));

        GetButton(Buttons.CloseButton).onClick.AddListener(() => { Close(Defines.UIAnimationType.Bounce); });
        GetButton(Buttons.ExitButton).onClick.AddListener(() => {  Application.Quit(); });
        GetToggle(Toggle.MuteToggle).onValueChanged.AddListener(OnMuteClick);

        GetSlider(Slider.BGMSlider).onValueChanged.AddListener(SetBGMVolume);
        GetSlider(Slider.SFXSlider).onValueChanged.AddListener(SetSFXVolume);
        GetSlider(Slider.MasterSlider).onValueChanged.AddListener(SetMasterVolume);

        return true;
    }
    
    public override void Open(Defines.UIAnimationType type = Defines.UIAnimationType.None)
    {
        base.Open(type);
        
        GetToggle(Toggle.MuteToggle).SetIsOnWithoutNotify(Managers.Sound.IsSoundOn);
        
        if (Managers.Sound.IsSoundOn)
        {
            GetSlider(Slider.BGMSlider).value = Managers.Sound.PrevSoundBgmValue;
            GetSlider(Slider.SFXSlider).value = Managers.Sound.PrevSoundSfxValue;
            GetSlider(Slider.MasterSlider).value = Managers.Sound.PrevSoundMasterValue;
        }
        else
        {
            GetSlider(Slider.BGMSlider).value = 0f;
            GetSlider(Slider.SFXSlider).value = 0f;
            GetSlider(Slider.MasterSlider).value = 0f;
        }

        Time.timeScale = 0;
    }
    
    public override void Close(Defines.UIAnimationType type = Defines.UIAnimationType.None)
    {
        base.Close(type);
        Time.timeScale = 1;
    }


    public void SetBGMVolume(float value)
    {
        Managers.Sound.SetBGMVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        Managers.Sound.SetSFXVolume(value);
    }


    public void SetMasterVolume(float value)
    {
        Managers.Sound.SetMasterVolume(value);
    }

    // 음소거 이벤트
    public void OnMuteClick(bool isOn)
    {
       
        if (isOn)
        {
            // 먼저 IsSoundOn 상태를 변경
            Managers.Sound.ToggleSound(true);
            GetSlider(Slider.BGMSlider).enabled = true;
            GetSlider(Slider.SFXSlider).enabled = true;
            GetSlider(Slider.MasterSlider).enabled = true;

            // 그 다음 슬라이더 값 변경
            GetSlider(Slider.BGMSlider).SetValueWithoutNotify(Managers.Sound.PrevSoundBgmValue);
            GetSlider(Slider.SFXSlider).SetValueWithoutNotify(Managers.Sound.PrevSoundSfxValue);
            GetSlider(Slider.MasterSlider).SetValueWithoutNotify(Managers.Sound.PrevSoundMasterValue);
        }
        else
        {
            // 현재 값 저장
            Managers.Sound.PrevSoundBgmValue = GetSlider(Slider.BGMSlider).value;
            Managers.Sound.PrevSoundSfxValue = GetSlider(Slider.SFXSlider).value;
            Managers.Sound.PrevSoundMasterValue = GetSlider(Slider.MasterSlider).value;
           
            // IsSoundOn 상태 변경
            Managers.Sound.ToggleSound(false);

            // 슬라이더 값을 0으로
            GetSlider(Slider.BGMSlider).enabled = false;
            GetSlider(Slider.BGMSlider).value = 0f;
            GetSlider(Slider.SFXSlider).enabled = false;
            GetSlider(Slider.SFXSlider).value = 0f;
            GetSlider(Slider.MasterSlider).enabled = false;
            GetSlider(Slider.MasterSlider).value = 0f;
            
            Managers.Sound.SetMasterVolume(0, false);
            Managers.Sound.SetBGMVolume(0, false);
            Managers.Sound.SetSFXVolume(0, false);
        }
    }

}
