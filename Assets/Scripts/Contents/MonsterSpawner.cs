using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner
{
    private List<List<Vector3>> spawnPoints;
    private List<WaveData> waveDatas;
    private Dictionary<int, GameObject> WorldMonster;
    private int Identifier;

    private int currentWave;
    private bool isSpawning;
    private int spawnCount;
    private bool waveEnd;

    /// <summary>
    /// 매개변수 : 완료된 웨이브 인덱스
    /// </summary>
    public event Action<int> OnClearWave;

    public void StartSpawn(LevelContainer levelContainer)
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

        SetWaveData();

        Managers.Coroutine.StartCoroutine("RunWaves", RunWaves());
    }

    private IEnumerator RunWaves()
    {
        for (int i = 0; i < waveDatas.Count; i++)
        {
            waveEnd = false;
            isSpawning = true;
            StartWave(i);
            yield return new WaitUntil(() => waveEnd);
            OnClearWave?.Invoke(i + 1);
        }
    }

    private void StartWave(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waveDatas.Count) return;

        Managers.Coroutine.StartCoroutine($"StartWave_{waveIndex}", RunSmallWaves(waveIndex));
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
        isSpawning = false;
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
        if (monster.Initialize(Identifier, monsterID) == false)
        {
            Managers.Pool.Despawn(go);
            return;
        }

        WorldMonster.Add(Identifier, go);
        monster.OnDead += Dead;
        monster.OnDisabled += Disabled;
        spawnCount++;
    }

    private void Dead(int identifier)
    {
        spawnCount--;
        if (isSpawning == false && spawnCount == 0)
        {
            waveEnd = true;
        }
    }

    private void Disabled(int identifier)
    {
        if (WorldMonster.TryGetValue(identifier, out GameObject go))
        {
            Managers.Pool.Despawn(go);
        }
    }












    // 웨이브 정보 하드코딩
    private void SetWaveData()
    {
        waveDatas = new List<WaveData>();

        int smallWaveCount;


        WaveData waveData0 = new WaveData();
        smallWaveCount = 3;
        waveData0.smallWaveDatas = new List<SmallWave>(smallWaveCount);
        for (int i = 0; i < smallWaveCount; i++)
        {
            waveData0.smallWaveDatas.Add(new SmallWave());
        }

        waveData0.smallWaveDatas[0].startInterval = 0;
        waveData0.smallWaveDatas[0].waveType = SmallWaveType.RandomPoint;
        waveData0.smallWaveDatas[0].pointGroup = 0;
        waveData0.smallWaveDatas[0].monsterID = 10001;
        waveData0.smallWaveDatas[0].monsterCount = 10;
        waveData0.smallWaveDatas[0].spawnInterval = 0.2f;

        waveData0.smallWaveDatas[1].startInterval = 5;
        waveData0.smallWaveDatas[1].waveType = SmallWaveType.AllPoint;
        waveData0.smallWaveDatas[1].pointGroup = 0;
        waveData0.smallWaveDatas[1].monsterID = 10001;
        waveData0.smallWaveDatas[1].monsterCount = 0;
        waveData0.smallWaveDatas[1].spawnInterval = 0;

        waveData0.smallWaveDatas[2].startInterval = 5;
        waveData0.smallWaveDatas[2].waveType = SmallWaveType.SequentialPoint;
        waveData0.smallWaveDatas[2].pointGroup = 0;
        waveData0.smallWaveDatas[2].monsterID = 10001;
        waveData0.smallWaveDatas[2].monsterCount = 0;
        waveData0.smallWaveDatas[2].spawnInterval = 0.2f;

        waveDatas.Add(waveData0);
    }
}


