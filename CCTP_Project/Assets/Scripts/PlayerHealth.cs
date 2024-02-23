using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float hitPoints = 100f;
    [SerializeField] Image healthBarFill;
    [SerializeField] private float startingHealth;

    private void Start()
    {
        startingHealth = hitPoints;
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        UpdateHealthBar();
        if(hitPoints <= 0)
        {
            GetComponent<DeathHandler>().HandleDeath();
        }
    }

    public void IncreaseHealth()
    {
        hitPoints = startingHealth;
        UpdateHealthBar();
    }

    public bool IsAtMaxHealth()
    {
        if(hitPoints == startingHealth)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateHealthBar()
    {
        if(healthBarFill != null)
        {
            healthBarFill.fillAmount = hitPoints / startingHealth;
        }
    }
}
