using TMPro;
using UnityEngine;

public class PlayerCombat : MonoBehaviour , IHealth
{
    [SerializeField] private float health = 3;
    [SerializeField] private float MaxDamageTaken = 3;
    
    public void TakeDamage(float damage)
    {
        if (damage > 0)
        {
            health -= 1;
        }
    }
}
