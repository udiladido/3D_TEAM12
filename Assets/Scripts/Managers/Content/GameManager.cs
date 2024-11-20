using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : IManager
{
    public Player Player {get; private set;}

    int monsterCount;
    private float elapsedTime;
    private float startTime;  
    private bool isWaveActive = false;
    private GameObject timerTextObject;
    private GameObject waveCounter;
    private GameObject countDownPopupUI;
    UICountDownPopup uICountDownPopup;
    

    //HashSet<MonsterDataList> monsterDataList = new HashSet<MonsterDataList>();

    public void Init()
    {
        // 게임시작하기 데이터 초기화 
        // 몬스터 데이터 연동, 웨이브 데이터 연동
        // 여기서 캐릭터 생성을 해야 하는가 아니면 캐릭터 선택창을 따로 만들 것인가
        startTime = Time.time;
        
        CreateTimer(); // 타이머 UI 텍스트 생성
        CreateWaveCounter();
        uICountDownPopup.CreateCountDown();
    }


    public void Clear()
    {
        // 데이터는 필요가 없음 데이터 제거 함수
        
    }

    public void GameStart()
    {
        // 실제로 게임을 시작하는 함수
        // 타이틀씬에서 버튼 연동 필요
        //isWaveActive = true;
        startTime = Time.time;
        elapsedTime = 0f;

        Managers.Scene.LoadScene(Defines.SceneType.GameScene);
    }

    public void GameOver()
    {
        // 실제로 게임이 종료되었을때 함수
        Managers.UI.ShowPopupUI<UIGameOverScene>();
    }

    public void Timer()
    {
        if (isWaveActive)
        {
            elapsedTime = Time.time - startTime; // 경과 시간 계산
            timerTextObject.GetComponentInChildren<Text>().text = Mathf.FloorToInt(elapsedTime).ToString();
        }
        else
        {
            timerTextObject.GetComponentInChildren<Text>().text = Mathf.FloorToInt(elapsedTime).ToString();
        }
    }

    public void UpdateWaveCounter()
    {
        //waveCounter.GetComponentInChildren<Text>().text = $"WAVE {monsterSpawner.waveCount}";
    }

    private void CreateTimer()
    {
        GameObject timerTextPrefab = Managers.Resource.Instantiate("Prefabs/UI/Popup/TimerTextPrefab"); // Resources 폴더에서 프리팹 로드
        timerTextObject = GameObject.Instantiate(timerTextPrefab); // 프리팹을 인스턴스화하여 타이머 텍스트 생성
    }

    private void CreateWaveCounter()
    {
        GameObject waveCounterPrefab = Managers.Resource.Instantiate("Prefabs/UI/Popup/WaveCounter"); // Resources 폴더에서 프리팹 로드
        waveCounter = GameObject.Instantiate(waveCounterPrefab); // 프리팹을 인스턴스화하여 타이머 텍스트 생성
    }

    public void CreatePlayer(int jobid)
    {
        JobEntity job = Managers.DB.Get<JobEntity>(11);

        Player player = GameObject.FindObjectOfType<Player>();
        if (player == null)
            player = Managers.Resource.Instantiate("Player")?.GetComponent<Player>();

        if (player == null)
        {
            Debug.LogWarning("Player 프리팹이 없습니다.");
            return;
        }
        player.gameObject.name = nameof(Player);
        player.SetJob(job);
        ItemEntity comboWeapon = Managers.DB.Get<ItemEntity>(1501);
        player.Equipment.Equip(comboWeapon);
        player.Condition.OnDead += GameOver;
        Player = player;
    }

    void Update()
    {
        Timer(); 
    }
}
