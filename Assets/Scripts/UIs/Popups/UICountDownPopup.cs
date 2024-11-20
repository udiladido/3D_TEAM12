using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UICountDownPopup : UIPopupBase
{
    private GameObject countDownPopupUI;
    string countDownPopupUIText;
    
    enum Texts
    {
        countDownText,
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));

        return true;
    }

    public override void Open(Defines.UIAnimationType type = Defines.UIAnimationType.None)
    {
        base.Open(type);
    }

    public void CreateCountDown()
    {
        countDownPopupUI = Managers.UI.ShowPopupUI<UICountDownPopup>().gameObject;
        countDownPopupUIText = countDownPopupUI.GetComponentInChildren<Text>().text;
        Managers.Coroutine.StartCoroutine("CountDown",CountDown());
    }

    private IEnumerator CountDown()
    {
        //Time.timeScale = 0;
        countDownPopupUIText = "3";
        yield return new WaitForSecondsRealtime(1);
        countDownPopupUIText = "2";
        yield return new WaitForSecondsRealtime(1);
        countDownPopupUIText = "1";
        yield return new WaitForSecondsRealtime(1);
        countDownPopupUIText = "시작!";
        yield return new WaitForSecondsRealtime(1);
        countDownPopupUI.SetActive(false);
        //Time.timeScale = 1f; // 게임 시작
    }
}
    

