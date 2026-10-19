using UnityEngine;

public class Test : MonoBehaviour, ITouch
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTouchStart()
    {
    }

    public void OnTouchContinue(Vector3 position)
    {
        Vector3 newPosition = transform.position;
        newPosition.x = position.x;
        transform.position = newPosition;
    }
}
