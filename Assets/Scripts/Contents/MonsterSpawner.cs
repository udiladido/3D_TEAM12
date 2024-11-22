using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner
{
    private List<List<Vector3>> spawnPoints;
    private List<WaveData> waveDatas;
    private Dictionary<int, GameObject> WorldMonster;
    private int Identifier;

    public int currentWave { get; private set; }
    public bool IsRunningWave { get; private set; }
    public bool IsSpawning { get; private set; }
    public int SpawnCount { get; private set; }

    /// <summary>
    /// 매개변수 : 완료된 웨이브 인덱스
    /// </summary>
    public event Action<int> OnClearWave;

    public event Action OnWaveAllClear;

    public void Initialize(LevelContainer levelContainer)
    {
        spawnPoints = new List<List<Vector3>>();
        List<Vector3> spawnPointsInner;
        foreach (MonsterSpawnPointGroup spawnPointGroup in levelContainer.MonsterSpawnPoints)
        {
            spawnPointsInner = new List<Vector3>();
            foreach (Transform transform in spawnPointGroup.Points)
            {
                spawnPointsInner.Add(transform.position);
            }
            spawnPoints.Add(spawnPointsInner);
            SmallWave.pointNum.Add(spawnPointsInner.Count);
        }
        SmallWave.SpawnFunc = SpawnEntity;

        WorldMonster = new Dictionary<int, GameObject>();
        Monster.MainCamera = Camera.main;

        SetWaveData();
    }

    public bool StartWave(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waveDatas.Count) return false;

        currentWave = waveIndex;
        IsRunningWave = true;
        IsSpawning = true;
        Monster.UnlimitRangeOfDetection = false;

        Managers.Coroutine.StartCoroutine($"StartWave_{waveIndex}", RunSmallWaves(waveIndex));

        return true;
    }
    private void EndWave()
    {
        IsRunningWave = false;
        OnClearWave?.Invoke(currentWave);

        if (currentWave == waveDatas.Count - 1)
        {
            OnWaveAllClear?.Invoke();
            return;
        }
        Managers.UI.ShowPopupUI<UIRewardPopup>();
    }


    private IEnumerator RunSmallWaves(int waveIndex)
    {
        List<SmallWave> smallWaveDatas = waveDatas[waveIndex].smallWaveDatas;

        for (int i = 0; i < smallWaveDatas.Count; i++)
        {
            yield return new WaitForSecondsRealtime(smallWaveDatas[i].startInterval);

            if (i == smallWaveDatas.Count - 1)
            {
                smallWaveDatas[i].Spawn(waveIndex, i, SpawnEndCollback);
            }
            else
            {
                smallWaveDatas[i].Spawn(waveIndex, i);
            }
        }
    }

    private void SpawnEndCollback()
    {
        IsSpawning = false;
        Monster.UnlimitRangeOfDetection = true;
    }


    private void SpawnEntity(int pointGroup, int point, int monsterID)
    {
        GameObject go = Managers.Pool.Spawn("/Monster");
        if (go == null)
        {
            return;
        }

        Monster monster = go.GetComponent<Monster>();
        if (monster == null)
        {
            Managers.Pool.Despawn(go);
            return;
        }

        Identifier++;
        Vector3 spawnPoint = spawnPoints[pointGroup][point];
        spawnPoint += new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        if (monster.Initialize(Identifier, monsterID, spawnPoint) == false)
        {
            Managers.Pool.Despawn(go);
            return;
        }

        WorldMonster.Add(Identifier, go);
        monster.OnDead += Dead;
        monster.OnDisabled += Disabled;
        SpawnCount++;
        Managers.Game.UpdateMonsterCounter();
    }

    private void Dead(int identifier)
    {
        SpawnCount--;
        Managers.Game.UpdateMonsterCounter();
        if (IsSpawning == false && SpawnCount == 0)
        {
            EndWave();
        }
    }

    private void Disabled(int identifier)
    {
        if (WorldMonster.TryGetValue(identifier, out GameObject go))
        {
            Managers.Pool.Despawn(go);
        }
    }

    // TODO : 웨이브 정보 데이터 리스트 만들기
    private void SetWaveData()
    {
        waveDatas = new List<WaveData>();

        foreach (var wave in Managers.DB.GetAll<WaveEntity>())
        {
            WaveData waveData = new WaveData(wave);
            waveDatas.Add(waveData);
        }
    }
}