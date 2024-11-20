using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterSelectPopup : UIPopupBase
{
    enum Texts
    {
        CharacterInfo,
    }

    enum Buttons
    {
        Babirain,
        Knight,
        Rogue,
        Mage
    }

    enum CharacterID
    {
        Babirain = 11,
        Knight,
        Rogue,
        Mage

    }


    protected override bool Init()
    {
        if (base.Init() == false)
            return false;



        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        GetButton(Buttons.Babirain).gameObject.BindEvent();
        GetButton(Buttons.Knight).gameObject.BindEvent();
        GetButton(Buttons.Rogue).gameObject.BindEvent();
        GetButton(Buttons.Mage).gameObject.BindEvent();

        return true;
    }


    private void SelectPlayer()
    {



    }

    


}
