using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 50f;

    private float currentHealth;
    private bool isDead = false;

    [Header("Effects (Optional)")]
    public GameObject hitEffect;
    public GameObject deathEffect;
    public EnemyAI enemyAI;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        // Optional hit effect
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        enemyAI.Death();
        // Optional death effect
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Notify SpawnManager (IMPORTANT for your spawn system)
        SpawnManager sm = Object.FindAnyObjectByType<SpawnManager>();
        if (sm != null)
        {
            sm.OnEnemyKilled();
        }
        
        // Destroy enemy
        Destroy(gameObject, 2.5f);
    }
}