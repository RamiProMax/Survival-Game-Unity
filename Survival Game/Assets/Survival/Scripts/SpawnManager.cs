using UnityEngine;
using TMPro; // If using TextMeshPro

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

    [Header("UI")]
    public TextMeshProUGUI waveText;

    void Start()
    {
        UpdateWaveUI();
        ApplyWaveSettings();
    }

    void Update()
    {
        if (!isSpawning) return;

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
        {
            return enemyPrefabs[0];
        }
        else if (currentWave <= 4)
        {
            return enemyPrefabs[Random.Range(0, 2)];
        }
        else
        {
            return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && enemiesSpawned >= enemiesPerWave)
        {
            NextWave();
        }
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

        ApplyWaveSettings();
        UpdateWaveUI();
    }

    void ApplyWaveSettings()
    {
        // Increase difficulty per wave
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