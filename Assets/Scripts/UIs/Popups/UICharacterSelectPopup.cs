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


        CharacterPreview preview = GameObject.FindObjectOfType<CharacterPreview>();
        if (preview == null)
            preview = Managers.Resource.Instantiate("CharacterPreview")?.GetComponent<CharacterPreview>();
        preview.gameObject.name = nameof(CharacterPreview);


        //버튼 이름 확인  - 캐릭터 ID 가져오기
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        CharacterID characterId = (CharacterID)Enum.Parse(typeof(CharacterID), clickedButton.gameObject.name);


        JobEntity job = Managers.DB.Get<JobEntity>((int)characterId);


        preview.SetJob(job);

        Managers.Game.SetPlayerJobId((int)characterId);
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


        Managers.Game.SetPlayerJobId((int)CharacterID.Barbarian);
        GetText(Texts.CharacterInfo).text = job.description;

    }

    /*

    private void SelectPrevCharacter_Knight()
    {


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

        GetImage(Images.Barbarian).sprite = Managers.Resource.Load<Sprite>(jobs[(int)Images.Barbarian].image);
        GetImage(Images.Knight).sprite = Managers.Resource.Load<Sprite>(jobs[(int)Images.Knight].image);
        GetImage(Images.Rogue).sprite = Managers.Resource.Load<Sprite>(jobs[(int)Images.Rogue].image);
        GetImage(Images.Mage).sprite = Managers.Resource.Load<Sprite>(jobs[(int)Images.Mage].image);
    }


    private void GameStart()
    {

        Managers.Scene.LoadScene(Defines.SceneType.GameScene);


    }


}