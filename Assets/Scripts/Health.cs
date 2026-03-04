using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent<int> OnHealthChanged;
    public UnityEvent OnDamaged;
    public UnityEvent OnDeath;

    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void AdjustHealth(int amount)
    {
        int originalHealth = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        if(originalHealth != currentHealth)
            OnHealthChanged.Invoke(currentHealth);

        if (currentHealth < originalHealth)
            OnDamaged.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetHealth(int newHealth)
    {
        int originalHealth = currentHealth;
        currentHealth = newHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        if(originalHealth != currentHealth)
            OnHealthChanged.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        if (currentHealth > maxHealth)
        {
            SetHealth(maxHealth);
        }

    }

    public void Die()
    {
        OnDeath.Invoke();
    }
}