using TMPro;
using UnityEngine;

public class PlayerCombat : MonoBehaviour , IHealth
{
    [SerializeField] private float health = 3;
    [SerializeField] private float MaxDamageTaken = 3;

    public float Health => health;
    
    public void TakeDamage(float damage)
    {
        if (damage > 0)
        {
            health -= 1;
            if (health <= 0)
            {
                health = 0;
                FindAnyObjectByType<CombatManager>().OnPlayerDeath();
            }
        }
    }
}
