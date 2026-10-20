using UnityEngine;

public class WorldMap : MonoBehaviour, ITouch
{
    [SerializeField] private Vector2 _minMaxScroll;
    private float delta;
    
    public void OnTouchStart(Vector3 position)
    {
        Vector3 newPosition = transform.position;
        delta = newPosition.y - position.y;
    }

    public void OnTouchEnd(Vector3 position)
    {
    }

    public void OnTouchContinue(Vector3 position)
    {
        Vector3 newPosition = transform.position;
        
        newPosition.y = position.y + delta;
        newPosition.y = Mathf.Clamp(newPosition.y, _minMaxScroll.x, _minMaxScroll.y);
        transform.position = newPosition;
    }
}
