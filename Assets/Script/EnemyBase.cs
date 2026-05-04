using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100f;
    public LayerMask groundLayer; 

    [Header("Patrol Settings")]
    public float walkSpeed = 2f;
    protected int direction = 1;

    [Header("Advanced Patrol")]
    public Transform wallCheck;
    public Transform ledgeCheck;
    public float wallCheckDistance = 0.5f; 
    public float ledgeCheckDistance = 1f;

    private float lastFlipTime;
    private float flipCooldown = 0.2f;

    [Header("Attack Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float attackRange = 7f;
    public float shootCooldown = 2f;
    public float launchForce = 10f;
    private float nextShootTime;

    [Header("UI References")]
    public Slider healthSlider; 

    [HideInInspector] public Transform player;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (healthSlider != null)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }
    }

    protected virtual void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            AttackPlayer();
        }
        else
        {
            Patrol();
        }
    }

    protected void Patrol()
    {
        rb.linearVelocity = new Vector2(direction * walkSpeed, rb.linearVelocity.y);

        bool hittingWall = Physics2D.Raycast(wallCheck.position, Vector2.right * direction, wallCheckDistance, groundLayer);
        bool isGroundedAhead = Physics2D.Raycast(ledgeCheck.position, Vector2.down, ledgeCheckDistance, groundLayer);

        if (hittingWall || !isGroundedAhead)
        {
            if (Time.time >= lastFlipTime + flipCooldown)
            {
                Flip();
            }
        }
    }

    protected void AttackPlayer()
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
        if (healthSlider != null) healthSlider.value = health;
        if (health <= 0) Die();
    }

    protected void Die() { Destroy(gameObject); }

    protected void Flip()
    {
        lastFlipTime = Time.time;
        direction *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected void LateUpdate()
    {
        if (healthSlider != null)
        {
            healthSlider.transform.rotation = Quaternion.identity;

            float parentDirection = transform.localScale.x;
            Vector3 localScale = healthSlider.transform.localScale;

            if (parentDirection < 0)
            {
                healthSlider.transform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);
            }
            else
            {
                healthSlider.transform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(wallCheck.position, Vector2.right * direction * wallCheckDistance);
        }
        if (ledgeCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(ledgeCheck.position, Vector2.down * ledgeCheckDistance);
        }
    }
}
