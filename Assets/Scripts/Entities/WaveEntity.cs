using System;
using System.Collections.Generic;

[Serializable]
public class WaveEntity : EntityBase
{
    public int waveSeq;
    public List<SmallWaveEntity> SmallWaveEntities;
}

[Serializable]
public class SmallWaveEntity
{
    public int waveId;
    public float startInterval;
    public Defines.SmallWaveType waveType;
    public int pointGroup;
    public int monsterID;
    public int monsterCount;
    public float spawnInterval;
}