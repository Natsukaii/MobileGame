using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string _currentSceneName = "";
    
    private MenuManager _menuManager;
    private LevelData _levelData;
    private int _currentLevelIndex;
    
    public static GameManager Instance { get; private set; }

    private const string UNLOCKED_LEVEL_KEY = "UnlockedLevelIndex";
    private const string DECK_KEY = "PlayerDeck";
    private const string INVENTORY_KEY = "PlayerInventory";


    public int UnlockedLevelIndex
    {
        get
        {
            int val = PlayerPrefs.GetInt(UNLOCKED_LEVEL_KEY, 0);
            return val;
        }
        set
        {
            PlayerPrefs.SetInt(UNLOCKED_LEVEL_KEY, value);
            PlayerPrefs.Save();
        }
    }

#region Money
    [SerializeField] private int money;
    [SerializeField] private int moneyPrenium;

    public int Money
    {
        get => money;
        set
        {
            money = value;
            if (_menuManager ==null)
                _menuManager = FindAnyObjectByType<MenuManager>();
            if (_menuManager != null)
                _menuManager.UpdateMoneyText(money);
        }
    }

    public int MoneyPrenium
    {
        get => moneyPrenium;
        set
        {
            moneyPrenium = value;
            if (_menuManager ==null)
                _menuManager = FindAnyObjectByType<MenuManager>();
            if (_menuManager != null)
                _menuManager.UpdateMoneyPreniumText(moneyPrenium);
        }
    }

#endregion

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        Application.targetFrameRate = 120;
    }
    
    void Start()
    {
        LoadLevel("Menu");
    }

    public void ResetGameData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        
        UnloadLevel(_currentSceneName);
        SceneManager.LoadScene("Main");
    }
    
    public void LoadLevel(string sceneName, LevelData levelData = null, int levelIndex = -1)
    {
        
        if (_currentSceneName != "" && _currentSceneName != sceneName)
        {
            try {
                SceneManager.UnloadSceneAsync(_currentSceneName);
            } catch (System.Exception e) {
            }
        }
            
        _currentSceneName = sceneName;
        Time.timeScale = 1;
        _levelData = levelData;
        
        if (levelIndex == -1 && levelData != null)
            _currentLevelIndex = levelData.LevelIndex;
        else
            _currentLevelIndex = levelIndex;
            
        StartCoroutine(LoadSceneCoroutine(sceneName, levelData));
    }
    public void UnloadLevel(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
    
    IEnumerator LoadSceneCoroutine(string sceneName, LevelData levelData) 
    {
        AsyncOperation operation =
            SceneManager.LoadSceneAsync(
                sceneName,
                LoadSceneMode.Additive
            );

        yield return operation;

        if (levelData != null)
        {
            if (levelData.LevelIndex == 0)
                FindAnyObjectByType<CombatManager>().StartCombat(levelData, true);
            else
                FindAnyObjectByType<CombatManager>().StartCombat(levelData);
        }
    }

    public void IsWin(bool isWin)
    {
        if (isWin)
        {
            if (_levelData != null)
                Money += _levelData.MoneyToWin;
            
            if (_currentLevelIndex != -1 && _currentLevelIndex >= UnlockedLevelIndex)
            {
                UnlockedLevelIndex = _currentLevelIndex + 1;
            }
        }
        else
        {
            
        }
        UnloadLevel(_currentSceneName);
        LoadLevel("Menu");
    }
}
