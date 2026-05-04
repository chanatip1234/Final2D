using UnityEngine;

public class IceDrop : MonoBehaviour
{
    [Header("Settings")]
    public float detectRange = 10f;    
    public float damage = 30f;        
    public LayerMask playerLayer;     

    private Rigidbody2D rb;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isFalling)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, detectRange, playerLayer);

            if (hit.collider != null)
            {
                Fall();
            }
        }
    }

    void Fall()
    {
        isFalling = true;
        rb.bodyType = RigidbodyType2D.Dynamic; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject); 
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject, 0.5f);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * detectRange);
    }
}
