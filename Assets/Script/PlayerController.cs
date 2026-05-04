using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Health System")]
    public float maxHealth = 100f;
    private float currentHealth;
    public Slider healthSlider;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Slide")]
    public float slideSpeed = 10f;
    public float slideTime = 0.5f;
    public float slideCooldown = 1f;

    [Header("Shooting")]
    public GameObject snowballPrefab;
    public Transform firePoint;
    public float launchForce = 10f;
    public float shootCooldown = 0.5f;
    private float nextShootTime = 0f;

    private bool isSliding;
    private bool canSlide = true;

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Animator anim;

    private bool isGrounded;
    private float moveInput;

    private Vector2 originalSize;
    private Vector2 originalOffset;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        originalSize = col.size;
        originalOffset = col.offset;

        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextShootTime)
        {
            ShootSnowball();
            nextShootTime = Time.time + shootCooldown;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && canSlide && !isSliding)
        {
            anim.SetTrigger("preSlide");
            StartCoroutine(Slide());
        }

        anim.SetFloat("speed", Mathf.Abs(moveInput));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding);

        FixHealthBarRotation();
    }

    void FixedUpdate()
    {
        if (!isSliding)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            float direction = transform.localScale.x;
            rb.linearVelocity = new Vector2(direction * slideSpeed, rb.linearVelocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    IEnumerator Slide()
    {
        isSliding = true;
        canSlide = false;

        col.size = new Vector2(originalSize.x, originalSize.y * 0.5f);
        col.offset = new Vector2(originalOffset.x, originalOffset.y - originalSize.y * 0.25f);

        float direction = transform.localScale.x;
        rb.linearVelocity = new Vector2(direction * slideSpeed, 0f);

        yield return new WaitForSeconds(slideTime);

        col.size = originalSize;
        col.offset = originalOffset;

        isSliding = false;

        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
    }
    void ShootSnowball()
    {
        anim.SetTrigger("attack");

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0;

        Vector2 direction = (Vector2)worldMousePos - (Vector2)firePoint.position;
        GameObject ball = Instantiate(snowballPrefab, firePoint.position, Quaternion.identity);

        ball.GetComponent<Snowball>().Launch(direction.normalized * launchForce + (Vector2.up * 2f));
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (healthSlider != null) healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Player Died!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void FixHealthBarRotation()
    {
        if (healthSlider != null)
        {
            healthSlider.transform.rotation = Quaternion.identity;

            float parentDirection = transform.localScale.x;
            Vector3 sliderScale = healthSlider.transform.localScale;

            if (parentDirection < 0)
            {
                healthSlider.transform.localScale = new Vector3(-Mathf.Abs(sliderScale.x), sliderScale.y, sliderScale.z);
            }
            else
            {
                healthSlider.transform.localScale = new Vector3(Mathf.Abs(sliderScale.x), sliderScale.y, sliderScale.z);
            }
        }
    }
    public void Heal(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        Debug.Log("Healed! Current Health: " + currentHealth);
    }
}
