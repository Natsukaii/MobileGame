using System.Collections;
using TMPro;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private TMP_Text _debugText;
    
    [Header("Configuration")]
    [SerializeField] private Transform[] _EnemiesSpawns;
    [SerializeField] private LevelData _levelData;
    [SerializeField] private float _timeBetweenWaves = 10;

    private CombatState _state;
    private float _timer;
    private float _timeToWait;

    public void StartCombat(LevelData levelData)
    {
        _levelData = levelData;
        SpawnNextWave();
    }
    
    private void Update()
    {
        _debugText.text = _state.ToString();
        _timer += Time.deltaTime;
        
        switch (_state)
        {
            case CombatState.Preparing:
                break;
            
            case CombatState.WaitingtWave:
                if (_timer >= _timeBetweenWaves)
                {
                    _timer = 0;
                    _state = CombatState.WaitingForNextWave;
                }
                break;
            
            case CombatState.WaitingForNextWave:
                if (_timer >= _timeBetweenWaves)
                {
                    _timer = 0;
                    SpawnNextWave();
                }
                break;
            
            case CombatState.End:
                break;
        }
        
    }

    private int _nextWaveIndex;
    private int _nextSpawnIndex;
    private int _spawnFrame = -1;
    private int _spawnsThisFrame;
    public void SpawnNextWave()
    {
        _state = CombatState.WaitingtWave;
        
        if (_levelData == null || _EnemiesSpawns == null || _EnemiesSpawns.Length == 0)
            return;

        if (_nextWaveIndex >= _levelData.waves.Count)
        {
            _debugText.text = "Toutes les vagues ont été lancées.";
            _state = CombatState.End;
            return;
        }

        WaveData wave = _levelData.waves[_nextWaveIndex];

        foreach (EnemyGroup group in wave.enemies)
        {
            if (group.prefab == null)
                continue;

            StartCoroutine(SpawnGroup(group));
        }

        _nextWaveIndex++;
    }

    private IEnumerator SpawnGroup(EnemyGroup group)
    {
        if (group.count <= 0)
            yield break;

        float[] spawnTimes = new float[group.count];
        float duration = Mathf.Max(0f, group.waveTime);
        int scheduled = 0;
        while (scheduled < group.count)
        {
            int batchSize = Random.Range(1, Mathf.Min(5, group.count - scheduled) + 1);
            float spawnTime = Random.Range(0f, duration);
            for (int i = 0; i < batchSize; i++)
                spawnTimes[scheduled++] = spawnTime;
        }

        System.Array.Sort(spawnTimes);
        float startTime = Time.time;

        foreach (float spawnTime in spawnTimes)
        {
            while (Time.time - startTime < spawnTime)
                yield return null;

            while (true)
            {
                if (_spawnFrame != Time.frameCount)
                {
                    _spawnFrame = Time.frameCount;
                    _spawnsThisFrame = 0;
                }

                if (_spawnsThisFrame < 5)
                    break;

                yield return null;
            }

            Transform spawn = GetNextSpawn();

            if (spawn != null)
            {
                _spawnsThisFrame++;
                Instantiate(group.prefab, spawn.position, spawn.rotation);
            }
        }
    }

    private Transform GetNextSpawn()
    {
        for (int i = 0; i < _EnemiesSpawns.Length; i++)
        {
            _nextSpawnIndex %= _EnemiesSpawns.Length;
            Transform spawn = _EnemiesSpawns[_nextSpawnIndex];
            _nextSpawnIndex = (_nextSpawnIndex + 1) % _EnemiesSpawns.Length;

            if (spawn != null)
                return spawn;
        }

        return null;
    }
}

enum CombatState
{
    Preparing,
    WaitingtWave,
    WaitingForNextWave,
    End
}
