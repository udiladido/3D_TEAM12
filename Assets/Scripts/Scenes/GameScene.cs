using System.Collections;
using UnityEngine;

public class GameScene : SceneBase
{
    protected override void OnSceneLoad()
    {
        // 1. 씬 로드시 필요한 로직을 수행
        Managers.UI.Init();
        Managers.Sound.Init();
        Managers.Pool.Init();
        Managers.Coroutine.Init();
        Managers.Game.Init();
    }


    protected override void OnSceneLoaded()
    {
        // 2. 씬 로드가 완료된 후 필요한 로직을 수행
        Managers.Sound.PlayBGM("BGM");
        Managers.Resource.Instantiate("Map/Level");
        Managers.UI.ShowPopupUI<UICountDownPopup>()?.StartCountDown();
    }
    protected override void OnSceneUnload()
    {
        // 3. 씬 언로드시 필요한 로직을 수행
        
    }
}