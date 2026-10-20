using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _debugText;

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
    
    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    
    public void LoadLevel(LevelData levelData)
    {
        LoadLevel("Game");
    }
    
    IEnumerator LoadSceneCoroutine(string sceneNam) 
    {
        AsyncOperation operation =
            SceneManager.LoadSceneAsync(
                sceneNam,
                LoadSceneMode.Additive
            );

        yield return operation;

        _debugText.text = "Scène chargée !";
    }
}
