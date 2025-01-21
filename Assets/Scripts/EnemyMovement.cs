using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 0.25f;
    private Vector3 targetPosition;
    public int health = 100;
    public string playerTag = "Player";
    public string stopLineTag = "Finish";

    private GameObject stopLine;
    private bool hasReachedStopline = false;
    private PillAttack pillAttack;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            targetPosition = player.transform.position;
        }
        else
        {
            Debug.LogError("Player GameObject with the specified tag not found!");
        }

        stopLine = GameObject.FindGameObjectWithTag(stopLineTag);

        if (stopLine == null)
        {
            Debug.LogError("Stop line GameObject with the specified tag not found!");
        }
        pillAttack = FindObjectOfType<PillAttack>();
    }

    void Update()
    {
        if (!hasReachedStopline)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (stopLine != null && transform.position.y <= stopLine.transform.position.y)
            {
                hasReachedStopline = true;
                NotifyPill(true);
                Debug.Log("Enemy reached the stopline.");
            }
        }
    }

    void OnDestroy()
    {
        if (hasReachedStopline)
        {
            NotifyPill(false); 
        }
    }

    void NotifyPill(bool isAdding)
    {
        PillAttack pill = FindObjectOfType<PillAttack>();
        if (pill != null)
        {
            if (isAdding)
            {
                pill.AddEnemyToStopline();
            }
            else
            {
                pill.RemoveEnemyFromStopline();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        if (pillAttack != null)
        {
            //reference to pill attack script
            pillAttack.AddXP(10); 
        }
        Destroy(gameObject);


    }
}
