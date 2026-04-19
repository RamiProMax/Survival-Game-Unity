using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunctions : MonoBehaviour
{
    private bool isPaused = false;

    // 🔴 Quit Game
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

        Application.Quit();

        // This only works in build, not in editor
    }

    // 🔄 Restart / Reload Current Scene
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
}