using UnityEngine;

public class UIGameScene : UISceneBase
{
    enum Objects
    {
        Status,
        Weapons,
    }
    
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind<GameObject>(typeof(Objects));

        return true;
    }
    
    public void HideUI()
    {
        GetObject(Objects.Status).SetActive(false);
        GetObject(Objects.Weapons).SetActive(false);
    }
    
    public void ShowUI()
    {
        GetObject(Objects.Status).SetActive(true);
        GetObject(Objects.Weapons).SetActive(true);
    }
}