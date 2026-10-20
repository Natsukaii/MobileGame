using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelData", menuName = "MobileGame/LevelData")]
public class LevelData : ScriptableObject
{
    public List<WaveData> waves = new List<WaveData>();
}

[Serializable]
public class WaveData
{
    public List<EnemyGroup> enemies = new List<EnemyGroup>();
}

[Serializable]
public class EnemyGroup
{
    public GameObject prefab;
    [Min(1)] public int count = 1;
    [Min(0f)] public float waveTime = 1;
}
