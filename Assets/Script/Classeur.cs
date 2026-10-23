using UnityEngine;

public class Classeur : MonoBehaviour
{
    [SerializeField] private ClasseurUI classeurUI;

    private void Start()
    {
        if (classeurUI == null)
        {
            classeurUI = GetComponentInChildren<ClasseurUI>();
        }

        if (classeurUI == null)
        {
            Debug.LogError("ClasseurUI n'est pas trouvé dans Classeur!");
        }
    }
}
