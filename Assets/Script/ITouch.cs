using UnityEngine;

public interface ITouch
{
    void OnTouchContinue(Vector3 position);
    void OnTouchStart();
}
