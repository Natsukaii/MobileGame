using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour , IHealth
{
    [SerializeField] private float health = 3;
    [SerializeField] private float MaxDamageTaken = 3;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private GameObject[] healthBar;

    public float Health => health;
    
    public void TakeDamage(float damage)
    {
        if (damage > 0)
        {
            StartCoroutine(FadeImage(healthBarImage, 0.3f, 0.2f));
            health -= 1;
            if (health > 0)
                healthBar[(int)health].SetActive(false);
            
            if (health <= 0)
            {
                health = 0;
                FindAnyObjectByType<CombatManager>().OnPlayerDeath();
            }
        }
    }
    
    public IEnumerator FadeImage(Image image, float targetAlpha, float duration)
    {
        Color color = image.color;
        float elapsed = 0f;

        // 0 -> targetAlpha
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, targetAlpha, elapsed / duration);
            image.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure we hit targetAlpha exactly
        image.color = new Color(color.r, color.g, color.b, targetAlpha);

        // targetAlpha -> 0
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(targetAlpha, 0f, elapsed / duration);
            image.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure we hit 0 exactly
        image.color = new Color(color.r, color.g, color.b, 0f);
    }
}
