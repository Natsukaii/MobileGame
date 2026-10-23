using System.Collections;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private GameObject _ui;
    [SerializeField] private GameObject _uiBooster;
    
    [SerializeField] private RectTransform _RotateTop;
    [SerializeField] private VfxAnimatorUI _vfxAnimator1;
    [SerializeField] private RectTransform[] _cards;
    
    [SerializeField] private float _bounceDuration = 0.5f;
    [SerializeField] private AnimationCurve _bounceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    void Start()
    {
        _uiBooster.SetActive(false);
        foreach (var card in _cards)
        {
            if (card != null) card.localScale = Vector3.zero;
        }
    }
    
    public void OpenBooster()
    {
        _uiBooster.SetActive(true);
        _ui.SetActive(false);
        foreach (RectTransform card in _cards)
        {
            SetCardScale(card, 0);
        }
    }

    public void OpenPack()
    {
        StartCoroutine(Rotate90Degrees(_RotateTop));
    }
    
    public void ShowCards()
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            if (_cards[i] != null)
            {
                StartCoroutine(BounceIn(_cards[i], i * 0.1f));
            }
        }
    }

    private void SetCardScale(RectTransform card, float scale)
    {
        card.localScale = new Vector3(scale, scale, 1);
    }

    private IEnumerator Rotate90Degrees(Transform target)
    {
        Quaternion startRotation = target.rotation;
        Quaternion endRotation = target.rotation * Quaternion.Euler(0, 0, -90);
        float duration = 0.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            target.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.rotation = endRotation;
        _vfxAnimator1.PlayAnimation("Booster");
        yield return new WaitForSeconds(1f);
        ShowCards();
    }
    
    private IEnumerator BounceIn(RectTransform target, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        target.localScale = Vector3.zero;
        float elapsed = 0;
        
        while (elapsed < _bounceDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _bounceDuration;
            
            float scale = 0;
            if (t < 0.7f)
            {
                scale = Mathf.Lerp(0, 1.2f, t / 0.7f);
            }
            else
            {
                scale = Mathf.Lerp(1.2f, 1.0f, (t - 0.7f) / 0.3f);
            }
            
            target.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
        
        target.localScale = Vector3.one;
    }

    public void ClosePack()
    {
        _RotateTop.rotation = Quaternion.identity;
        _uiBooster.SetActive(false);
        _ui.SetActive(true);
    }    
}
