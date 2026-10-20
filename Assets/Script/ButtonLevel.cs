using UnityEngine;

public class ButtonLevel : MonoBehaviour
{
    [SerializeField] LevelData levelData;
    public void loadLevel()
    {
        FindObjectsByType<GameManager>().LoadLevel(levelData);
    }
}
