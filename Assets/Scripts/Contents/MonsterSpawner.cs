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

    public int currentWave { get; private set; }
    public int spawnCount { get; private set; }
    private bool isSpawning;
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

        WorldMonster = new Dictionary<int, GameObject>();

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
        Vector3 spawnPoint = spawnPoints[pointGroup][point];
        if (monster.Initialize(Identifier, monsterID, spawnPoint) == false)
        {
            Managers.Pool.Despawn(go);
            return;
        }

        WorldMonster.Add(Identifier, go);
        monster.OnDead += Dead;
        monster.OnDisabled += Disabled;
        spawnCount++;

        Managers.Game.IncreaseMonsterCount();
    }

    private void Dead(int identifier)
    {
        spawnCount--;
        Managers.Game.DecreaseMonsterCount();

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












    // TODO : 웨이브 정보 데이터 리스트 만들기
    private void SetWaveData()
    {
        waveDatas = new List<WaveData>();

        WaveData waveData;
        SmallWave smallWave;



        waveData = new WaveData();
        waveData.smallWaveDatas = new List<SmallWave>();

        smallWave = new SmallWave();
        smallWave.startInterval = 0;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 2;
        smallWave.monsterID = 10001;
        smallWave.monsterCount = 10;
        smallWave.spawnInterval = 0.4f;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 5;
        smallWave.waveType = SmallWaveType.AllPoint;
        smallWave.pointGroup = 1;
        smallWave.monsterID = 10007;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 5;
        smallWave.waveType = SmallWaveType.SequentialPoint;
        smallWave.pointGroup = 0;
        smallWave.monsterID = 10001;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0.2f;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 5;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 3;
        smallWave.monsterID = 10003;
        smallWave.monsterCount = 3;
        smallWave.spawnInterval = 1;
        waveData.smallWaveDatas.Add(smallWave);

        waveDatas.Add(waveData);



        waveData = new WaveData();
        waveData.smallWaveDatas = new List<SmallWave>();

        smallWave = new SmallWave();
        smallWave.startInterval = 0;
        smallWave.waveType = SmallWaveType.SequentialPoint;
        smallWave.pointGroup = 1;
        smallWave.monsterID = 10001;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0.4f;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 2;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 2;
        smallWave.monsterID = 10003;
        smallWave.monsterCount = 6;
        smallWave.spawnInterval = 0.75f;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 1;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 0;
        smallWave.monsterID = 10001;
        smallWave.monsterCount = 20;
        smallWave.spawnInterval = 1f;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 5;
        smallWave.waveType = SmallWaveType.AllPoint;
        smallWave.pointGroup = 3;
        smallWave.monsterID = 10005;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 10;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 1;
        smallWave.monsterID = 10002;
        smallWave.monsterCount = 3;
        smallWave.spawnInterval = 0.5f;
        waveData.smallWaveDatas.Add(smallWave);

        waveDatas.Add(waveData);



        waveData = new WaveData();
        waveData.smallWaveDatas = new List<SmallWave>();

        smallWave = new SmallWave();
        smallWave.startInterval = 0;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 0;
        smallWave.monsterID = 10002;
        smallWave.monsterCount = 10;
        smallWave.spawnInterval = 2;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 0;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 1;
        smallWave.monsterID = 10001;
        smallWave.monsterCount = 20;
        smallWave.spawnInterval = 1;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 0;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 2;
        smallWave.monsterID = 10004;
        smallWave.monsterCount = 10;
        smallWave.spawnInterval = 2;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 0;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 3;
        smallWave.monsterID = 10005;
        smallWave.monsterCount = 15;
        smallWave.spawnInterval = 1.333f;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 21;
        smallWave.waveType = SmallWaveType.AllPoint;
        smallWave.pointGroup = 0;
        smallWave.monsterID = 10003;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 0;
        smallWave.waveType = SmallWaveType.AllPoint;
        smallWave.pointGroup = 3;
        smallWave.monsterID = 10003;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0;
        waveData.smallWaveDatas.Add(smallWave);

        waveDatas.Add(waveData);



        waveData = new WaveData();
        waveData.smallWaveDatas = new List<SmallWave>();

        smallWave = new SmallWave();
        smallWave.startInterval = 0;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 2;
        smallWave.monsterID = 10005;
        smallWave.monsterCount = 12;
        smallWave.spawnInterval = 0.3f;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 0;
        smallWave.waveType = SmallWaveType.SequentialPoint;
        smallWave.pointGroup = 1;
        smallWave.monsterID = 10001;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0.2f;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 2;
        smallWave.waveType = SmallWaveType.SequentialPoint;
        smallWave.pointGroup = 1;
        smallWave.monsterID = 10003;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0.1f;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 5;
        smallWave.waveType = SmallWaveType.AllPoint;
        smallWave.pointGroup = 2;
        smallWave.monsterID = 10003;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 8;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 3;
        smallWave.monsterID = 10007;
        smallWave.monsterCount = 1;
        smallWave.spawnInterval = 0;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 0.5f;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 0;
        smallWave.monsterID = 10004;
        smallWave.monsterCount = 6;
        smallWave.spawnInterval = 1f;
        waveData.smallWaveDatas.Add(smallWave);

        waveDatas.Add(waveData);



        waveData = new WaveData();
        waveData.smallWaveDatas = new List<SmallWave>();

        smallWave = new SmallWave();
        smallWave.startInterval = 0;
        smallWave.waveType = SmallWaveType.AllPoint;
        smallWave.pointGroup = 0;
        smallWave.monsterID = 10001;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 2;
        smallWave.waveType = SmallWaveType.AllPoint;
        smallWave.pointGroup = 1;
        smallWave.monsterID = 10003;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 2;
        smallWave.waveType = SmallWaveType.AllPoint;
        smallWave.pointGroup = 2;
        smallWave.monsterID = 10002;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 2;
        smallWave.waveType = SmallWaveType.AllPoint;
        smallWave.pointGroup = 3;
        smallWave.monsterID = 10005;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 5;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 0;
        smallWave.monsterID = 10004;
        smallWave.monsterCount = 10;
        smallWave.spawnInterval = 0.5f;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 3;
        smallWave.waveType = SmallWaveType.RandomPoint;
        smallWave.pointGroup = 2;
        smallWave.monsterID = 10006;
        smallWave.monsterCount = 10;
        smallWave.spawnInterval = 3;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 3;
        smallWave.waveType = SmallWaveType.SequentialPoint;
        smallWave.pointGroup = 1;
        smallWave.monsterID = 10002;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0.75f;
        waveData.smallWaveDatas.Add(smallWave);

        smallWave = new SmallWave();
        smallWave.startInterval = 3;
        smallWave.waveType = SmallWaveType.SequentialPoint;
        smallWave.pointGroup = 3;
        smallWave.monsterID = 10005;
        smallWave.monsterCount = 0;
        smallWave.spawnInterval = 0.5f;
        waveData.smallWaveDatas.Add(smallWave);

        waveDatas.Add(waveData);
    }
}


