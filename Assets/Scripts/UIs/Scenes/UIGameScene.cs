using System.Timers;
using TMPro;
using UnityEngine;

public class UIGameScene : UISceneBase
{
    enum Texts
    {
        WaveCounter,
        GameTime,
        MonsterCounter,
    }

    enum Objects
    {
        StatusBarSlots,
        EquippedSlots,
        QuickSlots,
    }

    private TMP_Text waveCounter;
    private TMP_Text gameTime;
    private TMP_Text monsterCounter;

    public float ElapsedTime { get; private set; }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind<GameObject>(typeof(Objects));
        BindText(typeof(Texts));

        ElapsedTime = 0f;
        SetWaveCounter(0);
        SetGameTime();
        SetMonsterCounter(0);

        return true;
    }

    private void Update()
    {
        ElapsedTime += Time.deltaTime;
        SetGameTime();
    }

    public void HideUI()
    {
        GetObject(Objects.StatusBarSlots).SetActive(false);
        GetObject(Objects.EquippedSlots).SetActive(false);
        GetObject(Objects.QuickSlots).SetActive(false);
    }

    public void ShowUI()
    {
        GetObject(Objects.StatusBarSlots).SetActive(true);
        GetObject(Objects.EquippedSlots).SetActive(true);
        GetObject(Objects.QuickSlots).SetActive(true);
    }

    public void SetWaveCounter(int wave)
    {
        if (waveCounter == null)
            waveCounter = GetText(Texts.WaveCounter);
        waveCounter.text = $"Wave : {wave}";
    }

    public void SetGameTime()
    {
        if (gameTime == null)
            gameTime = GetText(Texts.GameTime);
        
        gameTime.text = $"Time : {Mathf.FloorToInt(ElapsedTime > 60f ? ElapsedTime / 60 : ElapsedTime):00}";
    }

    public void SetMonsterCounter(int monsterCount)
    {
        if (monsterCounter == null)
            monsterCounter = GetText(Texts.MonsterCounter);
        
        monsterCounter.text = $"Monsters : {monsterCount:#,##0}";
    }
}