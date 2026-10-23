using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuyedCard : MonoBehaviour
{
    [SerializeField] private Image _card;
    [SerializeField] private float _bounceDuration = 1f;
    [SerializeField] private AnimationCurve _bounceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [SerializeField] private GameObject _ui;
    [SerializeField] private VfxAnimatorUI _vfxAnimator;
    [SerializeField] private VfxAnimatorUI _vfxAnimator2;
    
    public void OnBuy(Sprite card)
    {
        _card.sprite = card;
        //_card.rectTransform.localScale = new Vector3(0, 0, 1);
        BounceIn(_card.rectTransform, 0);
        _vfxAnimator.PlayAnimation("buy");
        _vfxAnimator2.PlayAnimation("buy");
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
}
