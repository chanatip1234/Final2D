using UnityEngine;
using UnityEngine.UI; // **ต้องเพิ่มบรรทัดนี้ครับ**

public class BossEnemy : EnemyBase
{
    public GameObject bossUI;
    private Slider bossSlider;

    protected override void Start()
    {
        base.Start(); 

        if (bossUI != null)
        {
            bossSlider = bossUI.GetComponent<Slider>();
            bossSlider.maxValue = health; 
            bossUI.SetActive(false); 
        }
    }

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

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
