using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : SceneBase
{
    protected override void OnSceneLoad()
    {

        Managers.UI.LoadSceneUI<UIIntroScene>();
        LoadPlayer();

    }

    protected override void OnSceneLoaded()
    {

  
    }

    protected override void OnSceneUnload()
    {
       
    }


    private void LoadPlayer()
    {
       
  
    }


}
