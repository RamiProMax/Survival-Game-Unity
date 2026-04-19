using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Regeneration Settings")]
    public float regenDelay = 3f;
    public float regenRate = 5f;

    private float lastDamageTime;
    private bool isDead = false;

    [Header("UI")]
    public Slider healthSlider;
    public GameObject gameOverPanel; // assign in inspector

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSound;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isDead) return;

        HandleRegen();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        lastDamageTime = Time.time;

        UpdateUI();

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void HandleRegen()
    {
        if (Time.time > lastDamageTime + regenDelay && currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player Dead");

        
        audioSource.PlayOneShot(deathSound);
    
        gameOverPanel.SetActive(true);
        

        // ⏸️ Pause game
        Time.timeScale = 0f;

        // 🖱️ Unlock cursor (important for UI)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}