using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : IManager
{
    public Player Player { get; private set; }
    public int JobId { get; private set; }

    private int monsterCount;
    private int waveCount;
    private float elapsedTime;
    private float startTime;

    private bool isWaveActive = false;
    private GameObject timerTextObject;
    private GameObject monsterCounter;
    private GameObject waveCounter;
    private GameObject countDownPopupUI;

    private MonsterSpawner monsterSpawner;

    public void Init()
    {
        // 게임시작하기 데이터 초기화 
        // 몬스터 데이터 연동, 웨이브 데이터 연동
        // 여기서 캐릭터 생성을 해야 하는가 아니면 캐릭터 선택창을 따로 만들 것인가
        startTime = Time.time;
        monsterCount = 0;
        waveCount = 0;
        isWaveActive = false;
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

        monsterSpawner = new MonsterSpawner();
        LevelContainer level = GameObject.FindFirstObjectByType<LevelContainer>();
        monsterSpawner.StartSpawn(level);
        monsterSpawner.OnClearWave += UpdateWaveCounter;
    }

    public void GameOver()
    {
        // 실제로 게임이 종료되었을때 함수
        Managers.UI.ShowPopupUI<UIGameOverPopup>();
    }

    public void CreatePlayer()
    {
        CreatePlayer(JobId);
    }
    public void CreatePlayer(int jobid)
    {
        JobEntity job = Managers.DB.Get<JobEntity>(jobid); //11

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

    public void UpdateWaveCounter(int waveCount)
    {
        if (monsterSpawner != null)
            Managers.UI.GetCurrentSceneUI<UIGameScene>()?.SetWaveCounter(waveCount);
    }

    public void UpdateMonsterCounter()
    {
        Managers.UI.GetCurrentSceneUI<UIGameScene>()?.SetMonsterCounter(monsterCount);
    }

    public void IncreaseMonsterCount()
    {
        monsterCount++;
        UpdateMonsterCounter(); // UI 업데이트
    }

    public void DecreaseMonsterCount()
    {
        if (monsterCount > 0)
        {
            monsterCount--;
            UpdateMonsterCounter(); // UI 업데이트
        }
    }

    public void ItemKeyPressed(int index)
    {
        if (Enum.IsDefined(typeof(Defines.ItemQuickSlotInputType), index))
        {
            Defines.ItemQuickSlotInputType inputType = (Defines.ItemQuickSlotInputType)index;
            Managers.Game.Player?.ItemQuickSlots?.Use(inputType);
        }
    }
    public void SetPlayerJobId(int jobId)
    {
        this.JobId = jobId;
    }
}