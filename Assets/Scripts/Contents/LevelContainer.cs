using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    public Transform PlayerStartPoint;
    public List<MonsterSpawnPointGroup> MonsterSpawnPoints;
}

[Serializable]
public class MonsterSpawnPointGroup
{
    public List<Transform> Points;
}
