using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UICharacterSelectPopup : UIPopupBase
{
    enum Texts
    {
        CharacterInfo,
    }

    enum Buttons
    {
        Barbarian = 0,
        Knight,
        Rogue,
        Mage,
        GameStartButton
    }

    enum Images
    {
        Barbarian,
        Knight,
        Rogue,
        Mage
    }


    enum CharacterID
    {
        Barbarian = 11,
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
        BindImage(typeof(Images));

        

        GetButton(Buttons.Barbarian).gameObject.BindEvent(SelectPrevCharacter);
        GetButton(Buttons.Knight).gameObject.BindEvent(SelectPrevCharacter);
        GetButton(Buttons.Rogue).gameObject.BindEvent(SelectPrevCharacter);
        GetButton(Buttons.Mage).gameObject.BindEvent(SelectPrevCharacter);
        GetButton(Buttons.GameStartButton).gameObject.BindEvent(GameStart);


        // 이미지 할당
        SetButtonImageData();


        // 기본 소환
        SelectPrevCharacter_Barbarian();

        return true;
    }



      private void SelectPrevCharacter() 
    {

        //이름으로 소환하기 
        CharacterPreview preview = GameObject.FindObjectOfType<CharacterPreview>();
        if (preview == null)
            preview = Managers.Resource.Instantiate("CharacterPreview")?.GetComponent<CharacterPreview>();
        preview.gameObject.name = nameof(CharacterPreview);



        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        CharacterID characterId = (CharacterID)Enum.Parse(typeof(CharacterID), clickedButton.gameObject.name);

    
        JobEntity job = Managers.DB.Get<JobEntity>((int)characterId);

        preview.SetJob(job);


        //Managers.Game. 에 jobid 변수 값 넘겨주기
        //Managers.Game.Jobid = (int)characterId;

        GetText(Texts.CharacterInfo).text = job.description;

    }


    private void SelectPrevCharacter_Barbarian()
    {

        CharacterPreview preview = GameObject.FindObjectOfType<CharacterPreview>();
        if (preview == null)
            preview = Managers.Resource.Instantiate("CharacterPreview")?.GetComponent<CharacterPreview>();
        preview.gameObject.name = nameof(CharacterPreview);


        JobEntity job = Managers.DB.Get<JobEntity>((int)CharacterID.Barbarian);
        preview.SetJob(job);


        //Managers.Game. 에 jobid 변수 값 넘겨주기
        //Managers.Game.Jobid = (int)characterId;

        GetText(Texts.CharacterInfo).text = job.description;

    }

    /*
    
    private void SelectPrevCharacter_Knight()
    {

        //이름으로 소환하기 
        CharacterPreview preview = GameObject.FindObjectOfType<CharacterPreview>();
        if (preview == null)
            preview = Managers.Resource.Instantiate("CharacterPreview")?.GetComponent<CharacterPreview>();
        preview.gameObject.name = nameof(CharacterPreview);

        JobEntity job = Managers.DB.Get<JobEntity>((int)CharacterID.Knight);

        preview.SetJob(job);


        //Managers.Game. 에 jobid 변수 값 넘겨주기
        //Managers.Game.Jobid = (int)characterId;

        GetText(Texts.CharacterInfo).text = job.description;

    }


    private void SelectPrevCharacter_Rogue()
    {

        //이름으로 소환하기 
        CharacterPreview preview = GameObject.FindObjectOfType<CharacterPreview>();
        if (preview == null)
            preview = Managers.Resource.Instantiate("CharacterPreview")?.GetComponent<CharacterPreview>();
        preview.gameObject.name = nameof(CharacterPreview);


        JobEntity job = Managers.DB.Get<JobEntity>((int)CharacterID.Rogue);

        preview.SetJob(job);


        //Managers.Game. 에 jobid 변수 값 넘겨주기
        //Managers.Game.Jobid = (int)characterId;

        GetText(Texts.CharacterInfo).text = job.description;

    }

    private void SelectPrevCharacter_Mage()
    {

        //이름으로 소환하기 
        CharacterPreview preview = GameObject.FindObjectOfType<CharacterPreview>();
        if (preview == null)
            preview = Managers.Resource.Instantiate("CharacterPreview")?.GetComponent<CharacterPreview>();
        preview.gameObject.name = nameof(CharacterPreview);


        JobEntity job = Managers.DB.Get<JobEntity>((int)CharacterID.Mage);

        preview.SetJob(job);


        //Managers.Game. 에 jobid 변수 값 넘겨주기
        //Managers.Game.Jobid = (int)characterId;

        GetText(Texts.CharacterInfo).text = job.description;

    }
     
     
     */


    public void SetButtonImageData()
    {

        List<JobEntity> jobs = Managers.DB.GetAll<JobEntity>();

      

        GetImage(Images.Barbarian).sprite = Managers.Resource.Load<Sprite>(jobs[0].image);
        GetImage(Images.Knight).sprite = Managers.Resource.Load<Sprite>(jobs[1].image);
        GetImage(Images.Rogue).sprite = Managers.Resource.Load<Sprite>(jobs[2].image);
        GetImage(Images.Mage).sprite = Managers.Resource.Load<Sprite>(jobs[3].image);
    }


    private void GameStart()
    {

        Managers.Scene.LoadScene(Defines.SceneType.GameScene);

    }



    


}
