using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using quiet;

public delegate void HealthEvent(HealthEventData healthEventData);

public class Health : MonoBehaviour
{
    [Header("Health Configuration")]
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int currentHealth;

    public int MaxHealth => maxHealth;
    public int CurrentHealth
    {
        get => currentHealth;
        // Never let currentHealth be negative or greater than max health
        private set => currentHealth = Mathf.Clamp(value, 0, MaxHealth);
    }

    /// <summary>
    /// Invoked when health reaches 0
    /// </summary>
    public event HealthEvent OnDeath;
    /// <summary>
    /// Invoked anytime health is taken from this gameObject
    /// </summary>
    public event HealthEvent OnDamage;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Causes this object to lose specified amount of health
    /// </summary>
    /// <param name="source"></param>
    /// <param name="damage"></param>
    public void TakeDamage(GameObject source, int damage)
    {
        CurrentHealth -= damage;

        HealthEventData hed = new HealthEventData(gameObject, source, damage);
        if(OnDamage != null)
        {
            OnDamage.Invoke(hed);
        }

        if(currentHealth <= 0)
        {
            if(OnDeath != null)
            {
                OnDeath.Invoke(hed);
            }
        }
    }

    public void SetMaxHealth(int max) => maxHealth = max;

    public void Heal(int hp) => CurrentHealth += hp;
}
