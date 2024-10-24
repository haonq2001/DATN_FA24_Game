using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 2f;
    public float jumpForce = 2f;
    private  Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer; // Biến để tham chiếu SpriteRenderer
    private Animator animator;
    public GameObject bullet;
    public Transform bulletPos;
    public static bool facingRight = true; // Sử dụng static để truy cập từ script khác

    // Thêm phương thức để cập nhật hướng nhân vật
    public static void UpdateFacingDirection(bool isFacingRight)
    {
        facingRight = isFacingRight;
        
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Lấy tham chiếu SpriteRenderer
        animator = GetComponent<Animator>(); // Lấy tham chiếu Animator
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Cast();
        Attack(); 
    }
    public void PlayerShoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletPos.position, Quaternion.identity);

        // Get the Bullet script from the bullet GameObject
        Bullet bulletScript = newBullet.GetComponent<Bullet>();

        Debug.Log("Player Facing Right: " + facingRight); // Debug the facing direction

        // Set direction based on player's facing direction
        if (!facingRight) // If the player is facing left
        {
            bulletScript.SetDirection(-1); // Set direction to left
        }
        else // If the player is facing right
        {
            bulletScript.SetDirection(1); // Set direction to right
        }
    }
  
        void Attack()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("attack"); // Gọi animation tấn công

        }
    }
    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput < 0) // Moving left
        {
            spriteRenderer.flipX = true; // Flip sprite to face left
            facingRight = false; // Update facing direction
        }
        else if (moveInput > 0) // Moving right
        {
            spriteRenderer.flipX = false; // Flip sprite to face right
            facingRight = true; // Update facing direction
        }

        animator.SetBool("isWalking", moveInput != 0);
        animator.SetBool("isGrounded", isGrounded); // Update grounded state
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;  // Đặt isGrounded thành false ngay khi nhảy
            animator.SetTrigger("Jump"); // Gọi animation nhảy
        }
    }
    void Cast()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("cast"); // Gọi animation chưởng
  
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra va chạm với mặt đất để xác định trạng thái isGrounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsGround", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsGround", false);
        }
    }
     private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("bay"))
        {
            animator.SetTrigger("die");
            StartCoroutine(WaitForDeathAnimation());
        }
    }
    IEnumerator WaitForDeathAnimation()
    {
        // Đợi 3 giây hoặc thời gian của animation chết
        yield return new WaitForSeconds(3f);

        // Sau khi chết xong, dừng màn hình
        Time.timeScale = 0; // Dừng thời gian
    }
}
