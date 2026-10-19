using UnityEngine;

public class StickerToSlider : MonoBehaviour , ITouch
{
    private Vector3 delta;
    
    public void OnTouchContinue(Vector3 position)
    {
        Vector3 newPosition = transform.position;
        
        newPosition = position + delta;
        newPosition.z = 0;
        transform.position = newPosition;
    }

    public void OnTouchStart(Vector3 position)
    {
        Vector3 newPosition = transform.position;
        delta = newPosition - position;
    }

    public void OnTouchEnd(Vector3 position)
    {
    }
}
