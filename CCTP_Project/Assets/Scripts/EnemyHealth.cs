using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    [SerializeField] Image healthBarFill;
    [SerializeField] private float startingHealth;

    GameObject HealthBarEnabled;
    [SerializeField] GameObject HealthBar;
    private bool HealthBarActive = true;

    bool isDead = false;

    //Collider enemyCollider;
    CapsuleCollider enemyCollider;

    private void Start()
    {
        enemyCollider = GetComponent<CapsuleCollider>();
        startingHealth = hitPoints;
        HealthBarEnabled = GameObject.FindGameObjectWithTag("EnemyHealthBar");
    }

    private void Update()
    {
        if(HealthBarEnabled == null)
        {
            HealthBarEnabled = GameObject.FindGameObjectWithTag("EnemyHealthBar");
        }
        if(HealthBarEnabled != null)
        {
            HealthBarActive = HealthBarEnabled.activeInHierarchy;
        }
        
        if(!HealthBarActive)
        {
            HealthBar.SetActive(false);
        }
        else
        {
            HealthBar.SetActive(true);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        BroadcastMessage("OnDamageTaken");
        hitPoints -= damage;
        UpdateHealthBar();
        if (hitPoints <= 0)
        {
            Die();
        }
    }

    public void UpdateHealthBar()
    {
        if(HealthBarActive)
        {
            if (healthBarFill != null)
            {
                healthBarFill.fillAmount = hitPoints / startingHealth;
            }
        }

    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        GetComponent<Animator>().SetTrigger("die");
        enemyCollider.direction = 2;
        enemyCollider.center = Vector3.zero;
    }
}
