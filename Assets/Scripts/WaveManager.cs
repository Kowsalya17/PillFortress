using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemies; // Enemy types for this wave
    public int[] enemyCounts; // Number of each enemy type
    public float spawnInterval; // Time between enemy spawns
}

public class WaveManager : MonoBehaviour
{
    public Wave[] waves; // Array of wave configurations
    public GameObject spawnArea; // The GameObject defining the spawn area
    public Transform target; // The target for enemies to move toward

    private int currentWaveIndex = 0;
    private int enemiesRemainingToSpawn;
    private int currentEnemyTypeIndex = 0;

    private float spawnTimer;

    void Start()
    {
        if (waves.Length > 0)
        {
            StartWave(currentWaveIndex);
        }
    }

    void Update()
    {
        if (enemiesRemainingToSpawn > 0)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                SpawnEnemy();
                spawnTimer = waves[currentWaveIndex].spawnInterval; // Reset the spawn timer
            }
        }
    }

    void StartWave(int waveIndex)
    {
        Wave wave = waves[waveIndex];
        enemiesRemainingToSpawn = 0;

        // Calculate total enemies to spawn in this wave
        foreach (int count in wave.enemyCounts)
        {
            enemiesRemainingToSpawn += count;
        }

        spawnTimer = 0f; // Start spawning immediately
        currentEnemyTypeIndex = 0;
    }

    void SpawnEnemy()
    {
        Wave wave = waves[currentWaveIndex];

        // If the current type is exhausted, move to the next type
        if (wave.enemyCounts[currentEnemyTypeIndex] <= 0)
        {
            currentEnemyTypeIndex++;
        }

        // If no more enemy types, stop spawning
        if (currentEnemyTypeIndex >= wave.enemies.Length)
        {
            return;
        }

        // Calculate random spawn position within the spawn area's bounds
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Spawn an enemy of the current type
        GameObject enemy = Instantiate(
            wave.enemies[currentEnemyTypeIndex],
            spawnPosition,
            Quaternion.identity
        );
       // enemy.GetComponent<EnemyMovement>().targetPosition = target.position;

        // Decrease remaining count of this enemy type
        wave.enemyCounts[currentEnemyTypeIndex]--;
        enemiesRemainingToSpawn--;

        // If wave is complete, move to the next wave
        if (enemiesRemainingToSpawn <= 0 && currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
            StartWave(currentWaveIndex);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Get the bounds of the spawn area
        Bounds bounds = spawnArea.GetComponent<Collider2D>().bounds;

        // Generate a random position within the bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(randomX, randomY, 0f);
    }
}
