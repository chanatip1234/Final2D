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
    }

    protected override void Update()
    {
        base.Update();
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