using UnityEngine;


public class Death : MonoBehaviour
{
    [SerializeField] private VfxAnimator _vfxAnimator;
    void Start()
    {
        _vfxAnimator.PlayAnimation("Death");
        Destroy(gameObject, 4);
    }
}
