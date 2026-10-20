using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _debugText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadLevel("Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
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
