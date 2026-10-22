using UnityEngine;
using UnityEngine.Serialization;

public class Enemie : MonoBehaviour , IHealth
{
    [SerializeField] private float life;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] [Min(0.1f)] private float recastAttaque;
    [SerializeField] private bool isRange;
    [SerializeField] private GameObject _missil;
    [SerializeField] private VfxAnimator _vfxAnimator;
    
    private float Timer = 0f;
    private Collider2D _collider;
    bool stopped = false;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        
        float detectionDistance = 0.5f; 
        if (_collider != null)
        {
            detectionDistance = (_collider.bounds.size.y / 2f) + 0.1f;
        }

        Vector2 rayStart = transform.position;
        float rayDistance = detectionDistance;
        
        if (_collider != null)
        {
            rayStart = (Vector2)transform.position + Vector2.down * (_collider.bounds.size.y / 2f + 0.01f);
            rayDistance = 0.1f; 
        }

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, rayDistance);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("StickerAlly") || hit.collider.CompareTag("StickerEnemi") || hit.collider.CompareTag("Player"))
            {
                stopped = true;
            }
            else
            {
                stopped = false;
            }
        }

        if (!stopped)
        {
            Move();
        }

        HandleAttack(hit.collider);
    }

    private void HandleAttack(Collider2D hitCollider)
    {
        Timer += Time.deltaTime;

        if (isRange)
        {
            if (Timer >= recastAttaque)
            {
                AttackRange();
                if (_vfxAnimator != null)
                {
                    _vfxAnimator.PlayAnimation("Missile");
                }
                Timer = 0f;
            }

            if (stopped && hitCollider != null && hitCollider.CompareTag("Player"))
            {
                IHealth health = hitCollider.GetComponent<IHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
        }
        else if (stopped && hitCollider != null)
        {
            if (hitCollider.CompareTag("StickerAlly") || hitCollider.CompareTag("Player"))
            {
                IHealth health = hitCollider.GetComponent<IHealth>();
                if (health != null && Timer >= recastAttaque)
                {
                    health.TakeDamage(damage);
                    Timer = 0f;
                    if (_vfxAnimator != null)
                    {
                        _vfxAnimator.PlayAnimation("Missile");
                    }
                }
                
                if (hitCollider.CompareTag("Player"))
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void AttackRange()
    {
        if (_missil != null)
        {
            GameObject missil = Instantiate(_missil, transform.position + Vector3.down * 0.5f, Quaternion.identity);
            missil.GetComponent<Missil>().SetSetting(damage, false, false);
        }
    }
    
    private void Move()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    public void TakeDamage(float damageAmount)
    {
        life -= damageAmount;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
