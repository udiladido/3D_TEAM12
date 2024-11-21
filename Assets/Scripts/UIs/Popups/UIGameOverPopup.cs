using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOverPopup : UIPopupBase
{
    enum Buttons
    {
        
        ReStartButton,
       
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton(Buttons.ReStartButton).gameObject.BindEvent(IntroSceneEvent);
        return true;
    }
    public override void Open(Defines.UIAnimationType type = Defines.UIAnimationType.None)
    {
        base.Open(type);
        Managers.Game.Player.Input.InputDisable();
        Time.timeScale = 0;
    }
 
    public void IntroSceneEvent()
    {
        Managers.Game.Player.Input.InputEnable();
        Time.timeScale = 1f;
        Managers.Scene.LoadScene(Defines.SceneType.IntroScene);
    }
}
