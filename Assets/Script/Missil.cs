using System;
using UnityEngine;

public class Missil : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Collider2D _collider;
    private float _damage;
    private bool _isAllyMissile;
    private bool _moveUp;
    
    private float _LifeTime = 10f;
    private float _timer = 0f;
    
   public void SetSetting(float damage, bool isAllyMissile, bool moveUp)
   {
        _damage = damage;
        _isAllyMissile = isAllyMissile;
        _moveUp = moveUp;
   }

    void Update()
    {
        Vector3 direction = _moveUp ? Vector3.up : Vector3.down;
        float moveDistance = speed * Time.deltaTime;

        if (moveDistance <= 0 && speed > 0)
        {
             return;
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, moveDistance + 0.1f);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject == gameObject) continue;

            if (HandleCollision(hit.collider.gameObject))
            {
                return;
            }
        }

        transform.Translate(direction * moveDistance);
        _timer += Time.deltaTime;
        if (_timer >= _LifeTime)
        {
            Destroy(gameObject);
        }
    }

    private bool HandleCollision(GameObject other)
    {
        bool hitSomething = false;
        if (_isAllyMissile)
        {
            if (other.CompareTag("StickerEnemi"))
            {
                ApplyDamage(other);
                hitSomething = true;
            }
        }
        else
        {
            if (other.CompareTag("StickerAlly"))
            {
                ApplyDamage(other);
                hitSomething = true;
            }
        }
        return hitSomething;
    }

    private void ApplyDamage(GameObject target)
    {
        IHealth health = target.GetComponent<IHealth>();
        if (health != null)
        {
            health.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
