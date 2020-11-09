using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using quiet;

public delegate void HealthEvent(HealthEventData healthEventData);

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int currentHealth;

    public int MaxHealth => maxHealth;
    public int CurrentHealth
    {
        get => currentHealth;
        private set => currentHealth = Mathf.Clamp(value, 0, MaxHealth);
    }

    public event HealthEvent OnDeath;
    public event HealthEvent OnDamage;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

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
