using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string _currentSceneName = "";
    
    private MenuManager _menuManager;
    private LevelData _levelData;

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
        Application.targetFrameRate = 120;
    }
    
    void Start()
    {
        LoadLevel("Menu");
        
        
        
    }

    void Update()
    {
        
    }
    
    public void LoadLevel(string sceneName, LevelData levelData = null)
    {
        if (_currentSceneName != "")
            SceneManager.UnloadSceneAsync(_currentSceneName);
        _currentSceneName = sceneName;
        Time.timeScale = 1;
        _levelData = levelData;
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
            FindAnyObjectByType<CombatManager>().StartCombat(levelData);
        }
    }

    public void IsWin(bool isWin)
    {
        if (isWin)
        {
            Money += _levelData.MoneyToWin;
        }
        else
        {
            
        }
        UnloadLevel(_currentSceneName);
        LoadLevel("Menu");
    }
}
