using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _debugText;
    private string _currentSceneName = "";
    
    private MenuManager _menuManager;


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
        StartCoroutine(LoadSceneCoroutine(sceneName, levelData));
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
        _debugText.text = "Scène chargée !";
    }
}
