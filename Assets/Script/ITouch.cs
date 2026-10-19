using UnityEngine;

public interface ITouch
{
    void OnTouchContinue(Vector3 position);
    void OnTouchStart(Vector3 position);
    void OnTouchEnd(Vector3 position);
}
