using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UICountDownPopup : UIPopupBase
{
    enum Texts
    {
        countDownText,
    }

    float startTime;

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

    //private IEnumerator CountDown()
    //{
    //    Time.timeScale = 0;
    //    countdownText.text = "3";
    //    startTime = Time.realtimeSinceStartup;
    //    yield return new WaitForSecondsRealtime(1);
    //    countdownText.text = "2";
    //    yield return new WaitForSecondsRealtime(1);
    //    countdownText.text = "1";
    //    yield return new WaitForSecondsRealtime(1);
    //    countdownText.text = "시작!";
    //    yield return new WaitForSecondsRealtime(1);
    //    countdownText.gameObject.SetActive(false);
    //    Time.timeScale = 1f; // 게임 시작
    //}
}
    

