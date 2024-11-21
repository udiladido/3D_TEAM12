using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class UICharacterSelectPopup : UIPopupBase
{
    enum Texts
    {
        CharacterInfo,
    }

    enum Buttons
    {
        Barbirian = 0,
        Knight,
        Rogue,
        Mage,
        CharacterCount,
        GameStartButton
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
        GetButton(Buttons.Barbirian).gameObject.BindEvent(SelectPrevCharacter);
        GetButton(Buttons.Knight).gameObject.BindEvent(SelectPrevCharacter);
        GetButton(Buttons.Rogue).gameObject.BindEvent(SelectPrevCharacter);
        GetButton(Buttons.Mage).gameObject.BindEvent(SelectPrevCharacter);
        GetButton(Buttons.GameStartButton).gameObject.BindEvent(GameStart);


        return true;
    }


    private void SelectPlayer()
    {

        //Todo : 

        //Managers.Game. 에 jobid 변수 값 넘겨주기

        //JobEntity job = Managers.DB.Get<JobEntity>(11);
        //GetText(Texts.CharacterInfo).text = job.description;
    }

    private void SelectPrevCharacter()
    {

        //이름으로 소환하기 
        CharacterPreview preview = GameObject.FindObjectOfType<CharacterPreview>();
        if (preview == null)
            preview = Managers.Resource.Instantiate("CharacterPreview")?.GetComponent<CharacterPreview>();
        preview.gameObject.name = nameof(CharacterPreview);

        CharacterID characterId = (CharacterID)Enum.Parse(typeof(CharacterID), preview.gameObject.name);

        JobEntity job = Managers.DB.Get<JobEntity>((int)characterId);

        preview.SetJob(job);


        //Managers.Game. 에 jobid 변수 값 넘겨주기
        //Managers.Game.Jobid = (int)characterId;

        GetText(Texts.CharacterInfo).text = job.description;

    }

    private void GameStart()
    {

        Managers.Scene.LoadScene(Defines.SceneType.GameScene);

    }



    


}
