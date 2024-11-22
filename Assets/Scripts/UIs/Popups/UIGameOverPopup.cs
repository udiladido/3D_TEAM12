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
        Managers.Game.StopGame();
    }
 
    public void IntroSceneEvent()
    {
        Managers.Game.ResumeGame();
        Managers.Scene.LoadScene(Defines.SceneType.IntroScene);
    }
}
