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
    
    float Timer = 0f;
    
    void start()
    {
        
    }

    void Update()
    {
        RaycastHit2D hit = RaycastHit2D.Raycast(transform.position, Vector2.down, 100f);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    
    private void Move()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        
    }
}
