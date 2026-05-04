using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [HideInInspector] public float damage;
    public float checkRadius = 0.5f; 
    public LayerMask playerLayer;    

    private void Update()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, checkRadius, playerLayer);

        if (hitPlayer != null)
        {
            if (hitPlayer.CompareTag("Player"))
            {
                PlayerController player = hitPlayer.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                    Debug.Log("Direct Hit with Overlap!");
                }
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Trap"))
        {
            Destroy(gameObject);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
