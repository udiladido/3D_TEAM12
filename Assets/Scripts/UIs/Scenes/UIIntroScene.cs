using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIntroScene : UISceneBase
{
    enum Buttons
    {
        CharacterSelectButton,
        QuitButton,
        SettingButton
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton(Buttons.CharacterSelectButton).gameObject.BindEvent(CharacterSelectButtonEvent);
        GetButton(Buttons.QuitButton).gameObject.BindEvent(QuitButtonEvent);
        GetButton(Buttons.SettingButton).gameObject.BindEvent(SettingPopupButtonEvent);
        return true;
    }


    public void CharacterSelectButtonEvent()
    {
    
        Managers.UI.ShowPopupUI<UICharacterSelectPopup>();
        gameObject.SetActive(false);
    }

    public void QuitButtonEvent()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //play모드를 false로.
#elif UNITY_WEBPLAYER
        Application.OpenURL("http://google.com"); //구글웹으로 전환
#else
        Application.Quit(); //어플리케이션 종료
#endif

    }

    public void SettingPopupButtonEvent()
    {

        Managers.UI.ShowPopupUI<UISettingPopup>();
    
    
    }

}
