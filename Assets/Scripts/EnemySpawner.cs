using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public string name;
        public GameObject prefab;
    }

    public EnemyType[] enemyTypes; 
    public Transform spawnPoint;   
    public TextMeshProUGUI waveText;
    // spawn wave interval between spawning
    private float[] waveIntervals = { 4f, 3f, 2f, 1f, 0.5f }; 
    // time delay bw spawning
    private float[] waveDurations = {15f, 12f, 10f, 5f, 2f }; 
    private int currentWave = 0;  
    private float spawnTimer;     
    private float waveTimer;      
    private float waveStartDelay = 10f; 
    private bool waveActive = false; 
    void Start()
    {
        if (waveIntervals.Length == 0 || waveDurations.Length == 0)
        {
            Debug.LogError("Wave intervals or durations are not defined!");
            return;
        }
        spawnTimer = waveIntervals[currentWave];
        waveTimer = 0f;
        UpdateWaveText();
    }

    void Update()
    {
        if (currentWave >= waveIntervals.Length)
        {
           // Debug.Log("All waves completed.");
            waveText.text = "All waves completed.";
            return; 
        }

        if (!waveActive && waveTimer >= waveStartDelay)
        {
            StartWave();
        }

        if (waveActive)
        {
            waveTimer += Time.deltaTime;
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0)
            {
                SpawnEnemy();
                spawnTimer = waveIntervals[currentWave]; 
            }

            if (waveTimer >= waveDurations[currentWave])
            {
                EndWave();
            }
        }
        else
        {
            waveTimer += Time.deltaTime; 
        }
    }

    void StartWave()
    {
        waveActive = true;
        waveTimer = 0f;
        Debug.Log($"Wave {currentWave + 1} started with {waveIntervals[currentWave]} seconds interval for {waveDurations[currentWave]} seconds.");
        UpdateWaveText();
    }

    void EndWave()
    {
        waveActive = false;
        waveTimer = 0f;
        currentWave++;

        if (currentWave < waveIntervals.Length)
        {
            Debug.Log($"Wave {currentWave + 1} will start after {waveStartDelay} seconds.");
        }
        else
        {
            Debug.Log("All waves completed.");
        }
    }

    void SpawnEnemy()
    {
        if (enemyTypes.Length == 0)
        {
            Debug.LogWarning("No enemy types assigned to the spawner!");
            return;
        }
        foreach (var enemyType in enemyTypes)
        {
            Collider2D spawnCollider = spawnPoint.GetComponent<Collider2D>();
            if (spawnCollider == null)
            {
                Debug.LogError("The spawnPoint GameObject doesn't have a Collider2D!");
                return;
            }

            float randomX = Random.Range(spawnCollider.bounds.min.x, spawnCollider.bounds.max.x);

            Vector3 spawnPosition = new Vector3(randomX, spawnPoint.position.y, spawnPoint.position.z);

            GameObject enemy = Instantiate(enemyType.prefab, spawnPosition, spawnPoint.rotation);
            Debug.Log($"Spawned {enemyType.name} at {spawnPosition}");
        }
    }

    void UpdateWaveText()
    {
        if (waveText != null)
        {
            if (currentWave < waveIntervals.Length)
            {
                waveText.text = $"Wave:{currentWave + 1}";
            }
            else
            {
                waveText.text = "All waves completed.";
            }
        }
    }
}
