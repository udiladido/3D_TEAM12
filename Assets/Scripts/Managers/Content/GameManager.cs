using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class GameManager : IManager
{
    int monsterCount;
    private float startTime;
    Condition condition;

    public void Init()
    {
        // 게임시작하기 전처리
        // 몬스터 데이터 연동, 웨이브 데이터 연동
        condition.OnDead += GameOver;
    }

    public void Clear()
    {
        // 데이터는 필요가 없음
        
    }

    public void GameStart()
    {
        // 실제로 게임을 시작하는 함수
        // 타이틀씬에서 버튼 연동 필요
        Managers.Scene.LoadScene(Defines.SceneType.GameScene);
        Managers.UI.ShowPopupUI<UICountDownPopup>();
    }

    public void GameOver()
    {
        // 실제로 게임이 종료되었을때 함수
        Managers.UI.ShowPopupUI<UIGameOverScene>();
    }

    public void UpdateWave()
    {

    }
}
