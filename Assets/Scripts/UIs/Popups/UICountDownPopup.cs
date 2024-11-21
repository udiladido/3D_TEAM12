using System.Collections;
using TMPro;
using UnityEngine;

public class UICountDownPopup : UIPopupBase
{
    TMP_Text countDownText;

    enum Texts
    {
        CountDownTxt,
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));

        return true;
    }

    public void StartCountDown()
    {
        countDownText = GetText(Texts.CountDownTxt);
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        //Time.timeScale = 0;
        countDownText.gameObject.SetActive(true);
        countDownText.text = "3";
        yield return new WaitForSecondsRealtime(1);
        countDownText.text = "2";
        yield return new WaitForSecondsRealtime(1);
        countDownText.text = "1";
        yield return new WaitForSecondsRealtime(1);
        countDownText.text = "시작!";
        yield return new WaitForSecondsRealtime(1);
        countDownText.gameObject.SetActive(false);
        //Time.timeScale = 1f; // 게임 시작
        
        Managers.Game.CreatePlayer();
        Managers.UI.LoadSceneUI<UIGameScene>();
        Managers.Game.GameStart();
    }
}
    

