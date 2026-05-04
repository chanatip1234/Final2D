using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [HideInInspector] public float damage;
 

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

        if (collision.CompareTag("Ground") || collision.CompareTag("Trap"))
        {
            Destroy(gameObject);
        }
    }
}
