using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class PillAttack : MonoBehaviour
{
    public float fireRate = 1f;
    public int pillHealth = 100;
    public int damagePerEnemy;
    public int xpPerHit = 10; 
    public int xpToLevelUp = 100; 

    private float fireCooldown = 0f;
    private int enemiesAtStopline = 0;
    private float healthReductionTimer = 0f;
    private TextMeshProUGUI health;
    private int enemies = 1;
    public GameObject projectilePrefab;
    public GameObject gameOverPanel;

    private int currentXP = 0; 
    public Slider xpSlider;

    private void Awake()
    {
        gameOverPanel = GameObject.Find("GameOver panel");
        gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        GameObject sliderObject = GameObject.Find("XP_Slider"); 
        if (sliderObject != null)
        {
            xpSlider = sliderObject.GetComponent<Slider>();
            xpSlider.maxValue = xpToLevelUp;
            xpSlider.value = currentXP;
            Debug.Log("XP Slider found and assigned.");
        }
        else
        {
            Debug.LogError("XP Slider not found in the scene!");
        }
        GameObject textObject = GameObject.Find("Health text");
        if (textObject != null)
        {
            health = textObject.GetComponent<TextMeshProUGUI>();
            health.SetText("100");
        }
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }

        if (enemiesAtStopline > 0)
        {
            healthReductionTimer += Time.deltaTime;

            if (healthReductionTimer >= 1f)
            {
                int totalDamage = enemies * damagePerEnemy;
                pillHealth -= totalDamage;
                health.SetText(pillHealth.ToString());
                Debug.Log($"Pill health reduced by {totalDamage} due to {enemiesAtStopline} enemies. Current health: {pillHealth}");

                healthReductionTimer = 0f;
            }
        }

        if (pillHealth <= 0)
        {
            Die();
        }
    }

    public void AddEnemyToStopline()
    {
        enemiesAtStopline++;
        Debug.Log($"Enemy added to stopline. Total enemies: {enemiesAtStopline}");
    }

    public void RemoveEnemyFromStopline()
    {
        enemiesAtStopline = Mathf.Max(0, enemiesAtStopline - 1);
        Debug.Log($"Enemy removed from stopline. Total enemies: {enemiesAtStopline}");
    }

    void Shoot()
    {
        GameObject targetEnemy = FindClosestEnemy();
        if (targetEnemy == null) return;

        Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;

        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        projectile.GetComponent<Rigidbody2D>().velocity = direction * 5f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
    public void AddXP(int xpAmount)
    {
        currentXP += xpAmount;

        if (xpSlider != null)
        {
            xpSlider.value = Mathf.Min(currentXP, xpToLevelUp);
        }

        Debug.Log($"XP Gained! Current XP: {currentXP}, Slider Value: {xpSlider.value}");

        if (currentXP >= xpToLevelUp)
        {
            pillHealth += 50;
            Debug.Log($"XP Full! Pill health increased by 50. Current Pill Health: {pillHealth}");
            currentXP = 0;

            if (xpSlider != null)
            {
                xpSlider.value = 0;
            }

            LevelUp();
        }

        UpdateHealthText();
    }
    void LevelUp()
    {
        Debug.Log("Level Up!");
    }
    void UpdateHealthText()
    {
        if (health != null)
        {
            health.SetText(pillHealth.ToString());
        }
    }

    void Die()
    {
        Debug.Log("The pill has been destroyed!");
        Destroy(gameObject);

        gameOverPanel.SetActive(true);
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
