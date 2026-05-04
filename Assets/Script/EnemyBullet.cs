using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 10f;

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
