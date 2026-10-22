using UnityEngine;

public class StickerAlly : MonoBehaviour, IHealth
{
    [SerializeField] private float _life = 10;
    [SerializeField] private float _recastAttaque = 1.5f;
    [SerializeField] private GameObject _missil;
    [SerializeField] private float damage = 1;
    
    [SerializeField] private VfxAnimator _vfxAnimator;
    
    private bool _isLocked = false;
    private float _timer = 0f;
    
    public bool IsLocked
    {
        get => _isLocked;
        set
        {
            _isLocked = value;
            gameObject.layer = _isLocked ? 0 : 6;
        }
    }

    public void TakeDamage(float damage)
    {
        _life -= damage;
        if (_life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        IsLocked = _isLocked;
    }
    
    private void Update()
    {
        if (!_isLocked) 
            return;
        
        _timer += Time.deltaTime;
        if (_timer >= _recastAttaque)
        {
            _timer = 0;
            if (_missil != null)
            {
                GameObject missil = Instantiate(_missil, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                Missil missilScript = missil.GetComponent<Missil>();
                if (missilScript != null)
                {
                    missilScript.SetSetting(damage, true, true);
                }
                if (_vfxAnimator != null)
                {
                    _vfxAnimator.PlayAnimation("Missile");
                }
            }
        }
    }
}
