using UnityEngine;

public class FishItem : MonoBehaviour
{
    public int scoreValue = 50; 

    [Header("Floating Settings")]
    public float amplitude = 0.1f;
    public float frequency = 2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

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
}
