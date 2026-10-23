using UnityEngine;

public class StickerAlly : MonoBehaviour, IHealth
{
    [SerializeField] private float _life = 10;
    [SerializeField] private float _recastAttaque = 1.5f;
    [SerializeField] private GameObject _missil;
    [SerializeField] private float damage = 1;
    
    [SerializeField] private VfxAnimator _vfxAnimator;
    [SerializeField] private GameObject _deathAnimation;
    
    private bool _isLocked = false;
    private float _timer = 0f;
    
    [SerializeField] private Transform spriteToShake;
    [SerializeField] private float magnitude = 0.02f;
    [SerializeField] private float smoothness = 10f;

    private Vector3 originalPosition = new Vector3();
    bool once = false;
    
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
            Instantiate(_deathAnimation, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        IsLocked = _isLocked;
        _timer = 0f;
    }
    
    private void Update()
    {
        if (!_isLocked)
        {
            Shake(true);
            return;
        }
        else
        {
            Shake(false);
        }
        
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

    public void Shake(bool isShaking)
    {
        if (isShaking)
        {
            Vector2 offset = Random.insideUnitCircle * magnitude;

            Vector3 targetPosition = originalPosition + new Vector3(offset.x, offset.y, 0f);

            spriteToShake.localPosition = Vector3.Lerp(spriteToShake.localPosition, targetPosition, Time.deltaTime * smoothness);

            once = false;
        }
        else if (!once)
        {
            spriteToShake.localPosition = Vector3.Lerp(spriteToShake.localPosition, originalPosition, Time.deltaTime * smoothness);

            if (Vector3.Distance(spriteToShake.localPosition, originalPosition) < 0.001f)
            {
                spriteToShake.localPosition = originalPosition;
                once = true;
            }
        }
    }
}
