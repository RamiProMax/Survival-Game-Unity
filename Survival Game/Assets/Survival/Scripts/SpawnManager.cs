using UnityEngine;
using TMPro;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("References")]
    public GameObject playerParent;
    public GameObject theMainCam;
    public Transform player;
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;

    [Header("Wave Settings")]
    public int currentWave = 1;
    public int totalWaves = 5;

    public int enemiesPerWave = 5;
    public float spawnInterval = 2f;
    public float spawnDistance = 20f;

    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;

    private float timer;
    private bool isSpawning = true;
    private bool waitingForUpgrade = false;

    [Header("Credits System")]
    public int credits = 0;
    public int creditsPerKill = 10;
    public TextMeshProUGUI creditsText;

    [Header("Score System")]
    public int score = 0;
    public TextMeshProUGUI scoreText1;
    public TextMeshProUGUI scoreText2;
    public TextMeshProUGUI scoreText3;

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public GameObject upgradePanel;
    public GameObject gameWonPanel;

    void Start()
    {
        UpdateWaveUI();
        UpdateCreditsUI();
        UpdateScoreUI();
        ApplyWaveSettings();

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        if (gameWonPanel != null)
            gameWonPanel.SetActive(false);
    }

    void Update()
    {
        if (!isSpawning || waitingForUpgrade) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval && enemiesSpawned < enemiesPerWave)
        {
            TrySpawn();
            timer = 0f;
        }
    }

    void TrySpawn()
    {
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

        float dist = Vector3.Distance(player.position, sp.position);

        if (dist > spawnDistance)
        {
            SpawnEnemy(sp);
        }
    }

    void SpawnEnemy(Transform spawnPoint)
    {
        GameObject enemyToSpawn = GetEnemyForWave();

        Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);

        enemiesSpawned++;
        enemiesAlive++;
    }

    GameObject GetEnemyForWave()
    {
        if (currentWave <= 1)
            return enemyPrefabs[0];
        else if (currentWave <= 3)
            return enemyPrefabs[Random.Range(0, 2)];
        else
            return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }

    // 🔥 UPDATED FUNCTION (takes score)
    public void OnEnemyKilled(int scoreValue)
    {
        enemiesAlive--;

        // 💰 Credits (still flat, you can also customize this later)
        credits += creditsPerKill;
        UpdateCreditsUI();

        // ⭐ Add dynamic score
        score += scoreValue;
        UpdateScoreUI();

        if (enemiesAlive <= 0 && enemiesSpawned >= enemiesPerWave)
        {
            WaveCompleted();
        }
    }

    void WaveCompleted()
    {
        isSpawning = false;
        waitingForUpgrade = true;

        if (currentWave >= totalWaves)
        {
            GameWon();
            return;
        }

        Time.timeScale = 0.3f;
        StartCoroutine(ShowUpgradePanelAfterDelay(2f));
    }

    IEnumerator ShowUpgradePanelAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 0f;

        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            playerParent.SetActive(false);
            theMainCam.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ContinueToNextWave()
    {
        Time.timeScale = 1f;

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        playerParent.SetActive(true);
        theMainCam.SetActive(false);

        waitingForUpgrade = false;

        NextWave();
        isSpawning = true;
    }

    void NextWave()
    {
        if (currentWave >= totalWaves)
        {
            GameWon();
            return;
        }

        currentWave++;
        enemiesSpawned = 0;
        enemiesAlive = 0;

        ApplyWaveSettings();
        UpdateWaveUI();
    }

    void GameWon()
    {
        Debug.Log("You Win!");

        isSpawning = false;
        Time.timeScale = 0f;

        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(true);
            playerParent.SetActive(false);
            theMainCam.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ApplyWaveSettings()
    {
        enemiesPerWave = 5 + (currentWave * 2);
        spawnInterval = Mathf.Max(0.5f, 2f - (currentWave * 0.2f));
    }

    void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave + " / " + totalWaves;
        }
    }

    void UpdateCreditsUI()
    {
        if (creditsText != null)
        {
            creditsText.text = "Scrap: " + credits;
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText1 != null)
            scoreText1.text = "Score: " + score;

        if (scoreText2 != null)
            scoreText2.text = "Score: " + score;
        if (scoreText3 != null)
            scoreText3.text = "Score: " + score;
    }
}