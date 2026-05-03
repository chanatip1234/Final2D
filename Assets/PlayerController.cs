using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
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
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // กระโดด
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // หันหน้า
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // Slide (Ctrl)
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && canSlide && !isSliding)
        {
            anim.SetTrigger("preSlide"); // 🔥 เล่นท่าก้มก่อน
            StartCoroutine(Slide());
        }

        // ส่งค่าไป Animator
        anim.SetFloat("speed", Mathf.Abs(moveInput));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding); // 🔥 สำคัญ
    }

    void FixedUpdate()
    {
        if (!isSliding)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    IEnumerator Slide()
    {
        isSliding = true;
        canSlide = false;

        // ลด collider (หมอบ)
        col.size = new Vector2(originalSize.x, originalSize.y * 0.5f);
        col.offset = new Vector2(originalOffset.x, originalOffset.y - originalSize.y * 0.25f);

        float direction = transform.localScale.x;
        rb.linearVelocity = new Vector2(direction * slideSpeed, 0f);

        yield return new WaitForSeconds(slideTime);

        // คืน collider
        col.size = originalSize;
        col.offset = originalOffset;

        isSliding = false;

        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
    }
}
