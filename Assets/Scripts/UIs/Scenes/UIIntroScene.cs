using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIntroScene : UISceneBase
{
    enum Buttons
    {
        StartButton,
        QuitButton,
        SettingButton
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton(Buttons.StartButton).gameObject.BindEvent(StartButtonEvent);
        GetButton(Buttons.QuitButton).gameObject.BindEvent(QuitButtonEvent);
        GetButton(Buttons.SettingButton).gameObject.BindEvent(SettingPopupButtonEvent);
        return true;
    }


    public void StartButtonEvent()
    {
        Managers.Scene.LoadScene(Defines.SceneType.GameScene);
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
