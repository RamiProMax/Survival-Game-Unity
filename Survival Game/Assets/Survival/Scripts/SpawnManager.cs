using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [Header("References")]
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

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public GameObject upgradePanel; // assign in inspector

    void Start()
    {
        UpdateWaveUI();
        ApplyWaveSettings();

        if (upgradePanel != null)
            upgradePanel.SetActive(false);
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
        if (currentWave <= 2)
            return enemyPrefabs[0];
        else if (currentWave <= 4)
            return enemyPrefabs[Random.Range(0, 2)];
        else
            return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && enemiesSpawned >= enemiesPerWave)
        {
            WaveCompleted();
        }
    }

    void WaveCompleted()
    {
        isSpawning = false;
        waitingForUpgrade = true;

        // ⏸️ Pause game
        Time.timeScale = 0f;

        // 🧾 Show upgrade UI
        if (upgradePanel != null)
            upgradePanel.SetActive(true);

        Debug.Log("Wave Complete - Open Upgrade Menu");
    }

    // 🎮 Call this from UI button
    public void ContinueToNextWave()
    {
        // ▶️ Resume game
        Time.timeScale = 1f;

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        waitingForUpgrade = false;

        NextWave();
        isSpawning = true;
    }

    void NextWave()
    {
        if (currentWave >= totalWaves)
        {
            Debug.Log("All waves completed!");
            isSpawning = false;
            return;
        }

        currentWave++;
        enemiesSpawned = 0;
        enemiesAlive = 0;

        ApplyWaveSettings();
        UpdateWaveUI();
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
}