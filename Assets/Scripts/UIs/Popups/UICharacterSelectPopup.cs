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
        Babirain = 0,
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

        SpawnPrevCharacter();

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        GetButton(Buttons.Babirain).gameObject.BindEvent();
        GetButton(Buttons.Knight).gameObject.BindEvent(SelectPlayer);
        GetButton(Buttons.Rogue).gameObject.BindEvent(SelectPlayer);
        GetButton(Buttons.Mage).gameObject.BindEvent(SelectPlayer);
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

    private void SpawnPrevCharacter()
    {


        // prefabs/Jobs에서 4개 다 소환 후 false하기

        for (int i = 0; i < (int)Buttons.CharacterCount; i++)
        {
            JobEntity job = Managers.DB.Get<JobEntity>(10+i);
            GameObject jobGo = Managers.Resource.Instantiate(job.prefabPath, transform);
            jobGo.SetActive(false);
        }

    }

    private void GameStart()
    {

        Managers.Scene.LoadScene(Defines.SceneType.GameScene);

    }



    


}
