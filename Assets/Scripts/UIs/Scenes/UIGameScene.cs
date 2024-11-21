using UnityEngine;

public class UIGameScene : UISceneBase
{
    enum Objects
    {
        StatusBarSlots,
        EquippedSlots,
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
        GetObject(Objects.StatusBarSlots).SetActive(false);
        GetObject(Objects.EquippedSlots).SetActive(false);
    }
    
    public void ShowUI()
    {
        GetObject(Objects.StatusBarSlots).SetActive(true);
        GetObject(Objects.EquippedSlots).SetActive(true);
    }
}