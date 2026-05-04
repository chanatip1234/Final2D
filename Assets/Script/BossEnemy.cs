using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : EnemyBase
{
    [Header("Boss UI Settings (Main Canvas)")]
    public GameObject bossUI;
    private Slider bossSlider;

    protected override void Start()
    {
        base.Start();
        scoreValue = 500;
    }

    
    protected override void Die()
    {
        
        if (GameManager.instance != null)
        {
            GameManager.instance.AddScore(scoreValue);

            
            GameManager.instance.ShowCredits();
        }
        
        Destroy(gameObject);

        
        if (bossUI != null) bossUI.SetActive(false);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (bossSlider != null)
        {
            bossSlider.value = health;
        }
    }
}