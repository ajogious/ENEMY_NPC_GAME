using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("UI")]
    public Slider healthSlider;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [HideInInspector]
    public bool isBlocking = false;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"{gameObject.name} starting with health: {maxHealth}");

        if (healthSlider == null)
        {
            Debug.LogWarning($"{gameObject.name}: Health slider not assigned!");
            return;
        }

        healthSlider.maxValue = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float amount, bool isTargetBlocking)
    {
        if (isTargetBlocking)
        {
            amount *= 0.25f; // Reduce incoming damage when blocking
        }

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        UpdateHealthUI();

        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");

        if (CompareTag("Player"))
        {
            // Handle player death — pause game or show Game Over screen
            Time.timeScale = 0f;
            // Optional: UIManager.Instance.ShowGameOver();
        }
        else
        {
            // Let enemy "flee" instead of being destroyed
            EnemyAI enemy = GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.RetreatAndHeal();
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} has no EnemyAI script attached.");
            }
        }
    }

    public float GetCurrentHealth() => currentHealth;
}
