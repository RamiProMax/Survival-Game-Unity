using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using XtremeFPS.FPSController;
using XtremeFPS.WeaponSystem;

public class UIFunctions : MonoBehaviour
{
    private bool isPaused = false;
    public UniversalWeaponSystem universalWeaponSystem;
    public FirstPersonController firstPersonController;
    public PlayerHealth playerHealth;
    public SpawnManager spawnManager;
    public Transform healthBar;
    public Transform sprintBar;

  
    public void QuitGame()
    {
        Debug.Log("Quit button pressed");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // stops play mode
#else
        Application.Quit(); // quits build
#endif
    }

    
    public void RestartScene()
    {
        Time.timeScale = 1f; // make sure game isn't paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ⏸️ Toggle Pause
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // ⏸️ Pause
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        Debug.Log("Game Paused");
    }

    // ▶️ Resume
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Game Resumed");
    }

    public void IncreaseBulletsBy(int amount)
    {
        
        if (amount == 150)
        {
            if(spawnManager.credits >= 50)
            {
                spawnManager.credits = spawnManager.credits- 50;
                universalWeaponSystem.totalBullets += amount;
                spawnManager.creditsText.text = "Scarp: " + spawnManager.credits;
            }     
        }
        else 
        {
            if(spawnManager.credits >= 100)
            {
                spawnManager.credits = spawnManager.credits - 100;
                universalWeaponSystem.totalBullets += amount;
                spawnManager.creditsText.text = "Scarp: " + spawnManager.credits;
            }
        }
    }

    public void IncreaseSprint()
    {   if(spawnManager.credits >= 70)
        {
            firstPersonController.sprintDuration = firstPersonController.sprintDuration*1.25f;
            Vector3 scale = sprintBar.localScale;
            scale.x += 0.3f;
            sprintBar.localScale = scale;
            spawnManager.credits = spawnManager.credits - 70;
            spawnManager.creditsText.text = "Scarp: " + spawnManager.credits;
        }
    }

    public void IncreasePlayerHealth()
    {
        if(spawnManager.credits >= 70)
        {
            playerHealth.maxHealth = playerHealth.maxHealth*1.30f;
            Vector3 scale = healthBar.localScale;
            scale.x += 0.3f;
            healthBar.localScale = scale;
            spawnManager.credits = spawnManager.credits - 70;
            spawnManager.creditsText.text = "Scarp: " + spawnManager.credits;
        }
    }
}