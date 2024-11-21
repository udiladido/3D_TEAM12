using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : IManager
{
    public Player Player {get; private set;}

    int monsterCount;
    int waveCount;
    private float elapsedTime;
    private float startTime;  
  
    private bool isWaveActive = false;
    private GameObject timerTextObject;
    private GameObject waveCounter;
    private GameObject countDownPopupUI;
    UICountDownPopup uiCountDownPopup;
    Text timerTextObjectText;

    //HashSet<MonsterDataList> monsterDataList = new HashSet<MonsterDataList>();

    public void Init()
    {
        // 게임시작하기 데이터 초기화 
        // 몬스터 데이터 연동, 웨이브 데이터 연동
        // 여기서 캐릭터 생성을 해야 하는가 아니면 캐릭터 선택창을 따로 만들 것인가
        startTime = Time.time;
        
        CreateTimer(); // 타이머 UI 텍스트 생성
        CreateWaveCounter();
        uiCountDownPopup.CreateCountDown();
    }

    public void Clear()
    {
        // 데이터는 필요가 없음 데이터 제거 함수

    }

    public void GameStart()
    {
        // 실제로 게임을 시작하는 함수
        // 타이틀씬에서 버튼 연동 필요
        
        startTime = Time.time;
        elapsedTime = 0f;

        timerTextObjectText = timerTextObject.GetComponentInChildren<Text>();
        Managers.Coroutine.StartCoroutine("UpdateTimer", UpdateTimer());
    }

    public void GameOver()
    {
        // 실제로 게임이 종료되었을때 함수
        Managers.UI.ShowPopupUI<UIGameOverScene>();

        if (!isWaveActive)
        {
            Managers.Coroutine.StopCoroutine("UpdateTimer");
        }
    }

    public void Timer()
    {
        if (isWaveActive)
        {
            elapsedTime = Time.time - startTime; // 경과 시간 계산    
        }

        timerTextObjectText.text = Mathf.FloorToInt(elapsedTime).ToString();
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

    public void CreatePlayer(int jobId)
    {
        JobEntity job = Managers.DB.Get<JobEntity>(jobid);

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

    private IEnumerator UpdateTimer()
    {
        WaitForSeconds wait1Sec = new WaitForSeconds(1f);

        while (isWaveActive)
        {
            Timer();
            yield return wait1Sec;
        }
    }
    
    public void ItemKeyPressed(int index)
    {
        if (System.Enum.IsDefined(typeof(Defines.ItemQuickSlotInputType), index))
        {
            Defines.ItemQuickSlotInputType inputType = (Defines.ItemQuickSlotInputType) index;
            Managers.Game.Player?.ItemQuickSlots?.Use(inputType);
        }
    }
}
