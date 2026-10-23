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
    [SerializeField] private GameObject _startWaveBtn;
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private VfxAnimatorUI _vfxAnimator;
    [SerializeField] private GameObject _defeatPanel;
    [SerializeField] private Transform[] _allySpawnPositions;

    private CombatState _state;
    private float _timer;
    private float _timeToWait;

    public void StartCombat(LevelData levelData)
    {
        _levelData = levelData;
        _state = CombatState.Preparing;
        _timer = 0;
    }

    
    private void Update()
    {
        _timer += Time.deltaTime;
        
        switch (_state)
        {
            case CombatState.Preparing:
                bool hasMoreWaves = _levelData != null && _nextWaveIndex < _levelData.waves.Count;
                
                if (AreAllEnemiesDead() && hasMoreWaves)
                {
                    if (!_startWaveBtn.activeSelf)
                    {
                        _startWaveBtn.SetActive(true);
                        SetAllAlliesLocked(false);
                    }
                }
                else if (!hasMoreWaves)
                {
                    if (_startWaveBtn.activeSelf)
                        _startWaveBtn.SetActive(false);
                    
                    if (AreAllEnemiesDead())
                    {
                        Win();
                    }
                }
                break;
            
            case CombatState.WaitingWave:
                if (_enemiesToSpawn <= 0 && AreAllEnemiesDead())
                {
                     _state = CombatState.Preparing;
                }
                break;
            
            case CombatState.End:
                break;
        }
    }

    private bool AreAllEnemiesDead()
    {
        return GameObject.FindGameObjectWithTag("StickerEnemi") == null;
    }

    private int _nextWaveIndex;
    private int _nextSpawnIndex;
    private int _spawnFrame = -1;
    private int _spawnsThisFrame;
    private float _wavetime;
    private int _enemiesToSpawn;
    
    public void SpawnNextWave()
    {
        if (!AreAllAlliesPlaced())
        {
            return;
        }

        _startWaveBtn.SetActive(false);
        SetAllAlliesLocked(true);
        _state = CombatState.WaitingWave;
        
        if (_levelData == null || _EnemiesSpawns == null || _EnemiesSpawns.Length == 0)
            return;

        if (_nextWaveIndex >= _levelData.waves.Count)
        {
            _state = CombatState.End;
            return;
        }

        WaveData wave = _levelData.waves[_nextWaveIndex];
        _wavetime = wave.waveTime;
        _enemiesToSpawn = 0;
        foreach (EnemyGroup group in wave.enemies)
        {
            if (group.prefab == null)
                continue;

            _enemiesToSpawn += group.count;
            StartCoroutine(SpawnGroup(group));
        }

        _nextWaveIndex++;
    }

    private IEnumerator SpawnGroup(EnemyGroup group)
    {
        if (group.count <= 0)
            yield break;

        float[] spawnTimes = new float[group.count];
        float duration = Mathf.Max(0f, _wavetime);
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
                _enemiesToSpawn--;
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

    private void SetAllAlliesLocked(bool locked)
    {
        StickerAlly[] allies = Object.FindObjectsByType<StickerAlly>(FindObjectsSortMode.None);
        foreach (var ally in allies)
        {
            ally.IsLocked = locked;
        }
    }

    private bool AreAllAlliesPlaced()
    {
        StickerAlly[] allies = Object.FindObjectsByType<StickerAlly>(FindObjectsSortMode.None);
        foreach (var ally in allies)
        {
            if (ally.transform.parent == null) return false;
        }
        return true;
    }

    public void OnPlayerDeath()
    {
        if (_state == CombatState.End) return;
        
        _state = CombatState.End;
        _debugText.text = "Défaite !";
        if (_defeatPanel != null) _defeatPanel.SetActive(true);
        //Time.timeScale = 0; 
    }

    private void Win()
    {
        if (_state == CombatState.End) return;

        _state = CombatState.End;
        if (_victoryPanel != null)
        {
            _victoryPanel.SetActive(true);
            _vfxAnimator.PlayAnimation("Win");
        }
        Debug.Log("Victoire !");
        // Time.timeScale = 0;
    }

    public void ButtonWinLose(bool isWin)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.IsWin(isWin);
        else
            FindAnyObjectByType<GameManager>().IsWin(isWin);
    }
}

enum CombatState
{
    Preparing,
    WaitingWave,
    End
}
