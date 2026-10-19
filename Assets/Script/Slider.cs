using UnityEngine;

public class Slider : MonoBehaviour, ITouch
{
    private float delta;
    
    public void OnTouchStart(Vector3 position)
    {
        Vector3 newPosition = transform.position;
        delta = newPosition.x - position.x;
    }

    public void OnTouchEnd(Vector3 position)
    {
    }

    public void OnTouchContinue(Vector3 position)
    {
        Vector3 newPosition = transform.position;
        
        newPosition.x = position.x + delta;
        transform.position = newPosition;
    }
}
