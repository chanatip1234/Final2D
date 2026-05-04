using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : EnemyBase
{
    [Header("Boss UI Settings")]
    public GameObject bossUI; 
    private Slider bossSlider;

    protected override void Start()
    {
        base.Start();

        if (bossUI != null)
        {
            bossSlider = bossUI.GetComponent<Slider>();
            bossSlider.maxValue = health;
            bossSlider.value = health;
            bossUI.SetActive(false); 
        }
    }

    protected override void Update()
    {
        base.Update(); 

        if (player == null || bossUI == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (!bossUI.activeSelf) bossUI.SetActive(true);
            bossSlider.value = health;
        }
        else
        {
            if (bossUI.activeSelf) bossUI.SetActive(false);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (bossUI != null && bossUI.activeSelf)
        {
            bossSlider.value = health;
        }
    }
}