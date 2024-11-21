using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WaveData
{
    public List<SmallWave> smallWaveDatas;
}


public enum SmallWaveType
{
    RandomPoint,
    AllPoint,
    SequentialPoint
}

[Serializable]
public class SmallWave
{
    public static List<int> pointNum = new List<int>();
    public static Action<int, int, int> SpawnFunc;

    public float startInterval;
    /// <summary>
    /// AllPoint : monsterCount,spawnInterval 무시
    /// SequentialPoint : monsterCount 무시
    /// </summary>
    public SmallWaveType waveType;
    public int pointGroup;
    public int monsterID;
    public int monsterCount;
    public float spawnInterval;

    private WaitForSecondsRealtime waitTime;
    private string coroutineKey;

    public void Spawn(int waveIndex, int smallWaveIndex, Action spawnEndCollback = null)
    {
        coroutineKey = $"Spawn_{waveIndex}_{smallWaveIndex}";

        switch (waveType)
        {
            case SmallWaveType.RandomPoint:
                waitTime = new WaitForSecondsRealtime(spawnInterval);
                Managers.Coroutine.StartCoroutine(coroutineKey, SpawnCoroutine_Random(spawnEndCollback));
                break;
            case SmallWaveType.AllPoint:
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
