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
        Time.timeScale = 0;
    }
 
    public void IntroSceneEvent()
    {
        Time.timeScale = 1f;
        Managers.Scene.LoadScene(Defines.SceneType.IntroScene);
    }
}
