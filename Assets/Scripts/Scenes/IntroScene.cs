using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : SceneBase
{
    protected override void OnSceneLoad()
    {
      
    }

    protected override void OnSceneLoaded()
    {

        Managers.UI.LoadSceneUI<UIIntroScene>();

    }

    protected override void OnSceneUnload()
    {
       
    }

  
}
