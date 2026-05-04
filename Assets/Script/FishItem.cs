using UnityEngine;

public class FishItem : MonoBehaviour
{
    public int scoreValue = 10; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(scoreValue);
            }
            Destroy(gameObject);
        }
    }
    void Update()
    {
        float newY = Mathf.Sin(Time.time * 2f) * 0.1f;
        transform.position = new Vector3(transform.position.x, transform.position.y + newY, transform.position.z);
    }
}
