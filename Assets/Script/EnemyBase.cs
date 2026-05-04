using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100f;
    public bool isBoss = false;

    [Header("Patrol Settings")]
    public float walkSpeed = 2f;
    public float patrolRange = 5f; 
    private Vector2 startPos;
    private int direction = 1;

    [Header("Attack Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float attackRange = 7f;
    public float shootCooldown = 2f;
    public float launchForce = 10f;
    private float nextShootTime;

    public Transform player;

    protected virtual void Start()
    {
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.Translate(Vector2.right * direction * walkSpeed * Time.deltaTime);

        if (Vector2.Distance(startPos, transform.position) >= patrolRange)
        {
            direction *= -1; 
            Flip();
        }
    }

    void AttackPlayer()
    {
        if (Time.time >= nextShootTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            Vector2 shootDir = (Vector2)player.position - (Vector2)firePoint.position;
            Vector2 finalVelocity = shootDir.normalized * launchForce + (Vector2.up * 3f);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = finalVelocity;

            nextShootTime = Time.time + shootCooldown;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    void Die()
    {  
        Destroy(gameObject);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
