using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WaveData
{
    public List<SmallWave> smallWaveDatas;
    
    public WaveData(WaveEntity entity)
    {
        smallWaveDatas = new List<SmallWave>();
        foreach (var smallWaveEntity in entity.SmallWaveEntities)
        {
            smallWaveDatas.Add(new SmallWave(smallWaveEntity));
        }
    }
}

[Serializable]
public class SmallWave
{
    public static List<int> pointNum = new List<int>();
    public static Action<int, int, int> SpawnFunc;

    public float startInterval;
    /// <summary>
    /// <para> RandomPoint : monsterID, monsterCount, spawnInterval </para>
    /// <para> AllPoint : monsterID </para>
    /// <para> SequentialPoint : monsterID, spawnInterval </para>
    /// </summary>
    public Defines.SmallWaveType waveType;
    public int pointGroup;
    public int monsterID;
    public int monsterCount;
    public float spawnInterval;

    private WaitForSecondsRealtime waitTime;
    private string coroutineKey;
    
    public SmallWave(SmallWaveEntity entity)
    {
        startInterval = entity.startInterval;
        waveType = entity.waveType;
        pointGroup = entity.pointGroup;
        monsterID = entity.monsterID;
        monsterCount = entity.monsterCount;
        spawnInterval = entity.spawnInterval;
    }

    public void Spawn(int waveIndex, int smallWaveIndex, Action spawnEndCollback = null)
    {
        coroutineKey = $"Spawn_{waveIndex}_{smallWaveIndex}";

        switch (waveType)
        {
            case Defines.SmallWaveType.RandomPoint:
                waitTime = new WaitForSecondsRealtime(spawnInterval);
                Managers.Coroutine.StartCoroutine(coroutineKey, SpawnCoroutine_Random(spawnEndCollback));
                break;
            case Defines.SmallWaveType.AllPoint:
                SpawnCoroutine_All();
                spawnEndCollback?.Invoke();
                break;
            default:
                waitTime = new WaitForSecondsRealtime(spawnInterval);
                Managers.Coroutine.StartCoroutine(coroutineKey, SpawnCoroutine_Sequential(spawnEndCollback));
                break;
        }
    }

    private IEnumerator SpawnCoroutine_Random(Action spawnEndCollback)
    {
        for (int i = 0; i < monsterCount; i++)
        {
            int point = Random.Range(0, pointNum[pointGroup]);
            SpawnFunc(pointGroup, point, monsterID);
            if (i == monsterCount - 1) spawnEndCollback?.Invoke();
            yield return waitTime;
        }
    }
    private void SpawnCoroutine_All()
    {
        for (int i = 0; i < pointNum[pointGroup]; i++)
        {
            SpawnFunc(pointGroup, i, monsterID);
        }
    }
    private IEnumerator SpawnCoroutine_Sequential(Action spawnEndCollback)
    {
        for (int i = 0; i < pointNum[pointGroup]; i++)
        {
            SpawnFunc(pointGroup, i, monsterID);
            if (i == pointNum[pointGroup] - 1) spawnEndCollback?.Invoke();
            yield return waitTime;
        }
    }
}
