using UnityEngine;
using UnityEngine.Serialization;

public class Enemie : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] [Min(0.1f)] private float recastAttaque;
    [SerializeField] private bool isRange;
    //[SerializeField] 
    [FormerlySerializedAs("collider")] [SerializeField] private ColliderEnemi colliderE;
    
    float Timer = 0f;
    
    void start()
    {
        
    }

    void Update()
    {
        
        
        if (isRange)
        {
            Timer += Time.deltaTime;
            if (Timer >= recastAttaque)
            {
                
            }
            if (colliderE._type != EnemiCollidetype.StickerAlly && colliderE._type != EnemiCollidetype.Player)
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
        }
        else
        {
            
        }
        
        
    }
    
    
}
