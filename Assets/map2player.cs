using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class map2player : MonoBehaviour
{
    private Vector3 originalScale;



    public GameObject chiakhoa;

    public GameObject keypostrai;  // Vị trí chìa khóa khi quay trái
    public GameObject keyposphai;
    private bool hasReceivedKey = false; // Kiểm tra xem người chơi đã nhận chìa khóa chưa

    private bool keyroundruong = false; // nhân vật có chia khóa chạm vào vào rương hay chưa
    public GameObject keyposmidtrai;
    public GameObject keyposmidphai;

    private bool isOnGround1 = false;
    public GameObject tang2;
    private bool isOnGround2 = false;
    public GameObject tang3;
    private bool isOnGround3 = false;
    public GameObject tang4;

    private bool isClimbing = false; // Trạng thái leo thang
    private float climbSpeed = 3f;  // Tốc độ leo thang 
    public float moveSpeed = 15f;
    public static bool facingRight = true;
    public GameObject skill3;
    public GameObject skill3mo;
    public GameObject skill4;
    public GameObject skill4mo;
    public GameObject skillpow;
    public GameManagermap2 gameManager;
    public Slider playerHealth;
    public Image fillImage;
    public Slider playerMana;
    public Image fillImagemana;
    public float moveNgang;
    public float moveDoc;
    public Vector2 movement;
    public Joystick joystick;

    public List<AudioClip> audioClips;
    private AudioSource audioSource;
 
    public audioManager audioManager;
    public float jumpForce = 2f;

    public GameObject bullet;
    public GameObject bulletpow;
    public Transform bulletPos;
    private Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public float castCooldown = 2f;
    public float castCooldownpow = 5f;
    private float lastCastTime = 0f;
    public GameObject swordCollider;
    public GameObject swordCollider1;
    public GameObject swordCollider2;
    public int torchCount = 0;  // Biến để lưu trữ số ngọn lửa (hoặc đuốc) của người chơi
    public int health = 10;
    public int mana = 10;


    public Image[] buttonImages; // Biến để tham chiếu đến Image của button
    // Start is called before the first frame update
    void Start()
    {
        chiakhoa.SetActive(false);
        skill4.SetActive(false);
        tang2.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Gán Animator từ component 
        skill3mo.SetActive(false);
        skill3.SetActive(true);
        skillpow.SetActive(false);
        skillpow.SetActive(false);

        swordCollider.SetActive(false);
        swordCollider1.SetActive(false);
        swordCollider2.SetActive(false);

        playerHealth.interactable = false;
        playerMana.interactable = false;
        playerHealth.maxValue = health;

        playerMana.maxValue = mana;
        playerHealth.value = Mathf.Clamp(health, 0, playerHealth.maxValue);
        playerMana.value = Mathf.Clamp(mana, 0, playerMana.maxValue);

        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
      //  Attack();
        if (isOnGround1) // Kiểm tra nếu đang trên `ground1`
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                tang2.SetActive(true);
                Debug.Log("Bật tầng 2");
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                tang2.SetActive(false);
                Debug.Log("Tắt tầng 2");
            }
        }
        if (isOnGround2) // Kiểm tra nếu đang trên `ground2`
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                tang3.SetActive(true);
                Debug.Log("Bật bac thang tầng 3");
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                tang3.SetActive(false);
                Debug.Log("Tắt bac thang tang 3");
            }
            if (isOnGround3 && Input.GetKeyDown(KeyCode.W))
            {

             //   StartCoroutine(thoigiannhaycaonhat());

            }
        }
        if (playerMana.value > 0)
        {
            //Attack1();
            //Cast();
            //Attack3();
            //CastPow();
        }
        else
        {
            Debug.Log("Không đủ năng lượng để sử dụng kĩ năng");
        }


        // Kiểm tra mana và cập nhật tất cả các button
        foreach (Image buttonImage in buttonImages)
        {
         //   UpdateButton(buttonImage);
        }
        // Di chuyển ngang
        float moveHorizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveHorizontal * climbSpeed, rb.velocity.y);

        // Kiểm tra leo thang
        if (isClimbing)
        {
            float moveVertical = Input.GetAxis("Vertical"); // Nhấn phím lên/xuống
            rb.velocity = new Vector2(rb.velocity.x, moveVertical * climbSpeed);

            rb.gravityScale = 0; // Vô hiệu hóa trọng lực khi leo thang
            animator.SetBool("IsClimbing", true); // Gắn cờ leo thang trong Animator
                                                  // Kết hợp animation đi bộ khi nhân vật di chuyển trên cầu thang

        }
        else
        {
            rb.gravityScale = 1; // Khôi phục trọng lực khi không leo thang
            animator.SetBool("IsClimbing", false);


        }

    }
    void Move()
    {
        // Lấy giá trị từ joystick và bàn phím
        float moveInput = joystick.Horizontal + Input.GetAxis("Horizontal");

        // Ngưỡng để bỏ qua giá trị rất nhỏ từ joystick
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            // Đổi hướng nhân vật
            if (moveInput < 0)
            {
                spriteRenderer.flipX = true;
                facingRight = false;
            }
            else if (moveInput > 0)
            {
                spriteRenderer.flipX = false;
                facingRight = true;
            }

            animator.SetBool("isWalking", isGrounded); // Animation "đi bộ" chỉ kích hoạt nếu đang chạm đất
        }
        else
        {
            // Chỉ dừng vận tốc ngang nếu đang chạm đất
            if (isGrounded)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetBool("isWalking", false); // Ngừng animation đi bộ
            }
        }
    }

    void Jump()
    {
        if ((joystick.Vertical > 0.5f || Input.GetButtonDown("Jump")) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false; // Đặt trạng thái "ở trên không"
            animator.SetTrigger("Jump");
            audioManager.Instance.PlaySFX("jump");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {

            isGrounded = true;
            Debug.Log("Chạm đất!");
            animator.SetBool("IsGround", true);
            //isColliderActive=true;
            //    if (boxCollider2D != null)
            //    {
            //        boxCollider2D.enabled = true;
            //    }



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

}
