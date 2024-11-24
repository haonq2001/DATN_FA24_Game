using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class playermap2 : MonoBehaviour
{

     private bool isOnGround1= false;
    public GameObject tang2;

    private bool isClimbing = false; // Trạng thái leo thang
    private float climbSpeed = 3f;  // Tốc độ leo thang 

    //   private PolygonCollider2D boxCollider2D;
    // private  bool isColliderActive= true;
    public GameObject skill3;
    public GameObject skill3mo;

    public GameManager gameManager;

    public Slider playerHealth;
    public Image fillImage;
    public Slider playerMana;
    public Image fillImagemana;




    public float moveSpeed = 2f;

    public float moveNgang;
    public float moveDoc;
    public Vector2 movement;
    public Joystick joystick;



    // am thanh
    public List<AudioClip> audioClips;
    private AudioSource audioSource;
    public GameObject batnhacnen;
    public GameObject tatnhacnen;
    public GameObject batamthanh;
    public GameObject tatamthanh;
    public audioManager audioManager;




    public float jumpForce = 2f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public GameObject bullet;
    public Transform bulletPos;
    public static bool facingRight = true;
    private float castCooldown = 2f;
    private float lastCastTime = 0f;
    public GameObject swordCollider;
    public GameObject swordCollider1;
    public GameObject swordCollider2;
    public int torchCount = 0;  // Biến để lưu trữ số ngọn lửa (hoặc đuốc) của người chơi
    public int health = 10;
    public int mana = 10;


    public Image[] buttonImages; // Biến để tham chiếu đến Image của button

    void Start()
    {


        tang2.SetActive(false);
        //  boxCollider2D = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Gán Animator từ component

        skill3mo.SetActive(false);
        skill3.SetActive(true);

        swordCollider.SetActive(false);
        swordCollider1.SetActive(false);
        swordCollider2.SetActive(false);

        playerHealth.maxValue = health;
        playerHealth.value = health;
        playerMana.maxValue = mana;
        playerMana.value = mana;

        audioSource = GetComponent<AudioSource>();

    }
    // bh fix loi xong them chu d vao
    void FixeUpdate()
    {
        // lay gia tri tu ban phim
        moveNgang = Input.GetAxis("Horizontal");
        moveDoc = Input.GetAxis("Vertical");

        moveNgang = joystick.Horizontal;
        moveDoc = joystick.Vertical;

        movement = new Vector2(moveNgang, moveDoc) * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void Update()
    {
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
        Move();
        Jump();
        Attack();


        // Kiểm tra nhấn phím Space để bắn đạn
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    PlayerShoot();
        //}
        if (playerMana.value > 0)
        {
            Attack1();
            Cast();
            Attack3();
        }
        else
        {
            Debug.Log("Không đủ năng lượng để sử dụng kĩ năng");
        }


        // Kiểm tra mana và cập nhật tất cả các button
        foreach (Image buttonImage in buttonImages)
        {
            UpdateButton(buttonImage);
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




    void UpdateButton(Image button)
    {
        if (playerMana.value <= 0)
        {
            Color color = button.color;  // Truy cập thuộc tính color của Image
            color.a = 0.5f; // Làm mờ button
            button.color = color;
            button.GetComponent<Button>().interactable = false; // Tắt button
        }
        else
        {
            Color color = button.color;  // Truy cập thuộc tính color của Image
            color.a = 1f; // Đặt lại độ sáng
            button.color = color;
            button.GetComponent<Button>().interactable = true; // Bật lại button
        }
    }

    public void PlayerShoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        Bullet bulletScript = newBullet.GetComponent<Bullet>();

        Debug.Log("Player Facing Right: " + facingRight);

        if (!facingRight)
        {
            bulletScript.SetDirection(-1);
        }
        else
        {
            bulletScript.SetDirection(1);
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("attack");
            audioManager.Instance.PlaySFX("skill1");
        }
    }
    public void buttonattack()
    {
        animator.SetTrigger("attack");
        audioManager.Instance.PlaySFX("skill1");
    }

    void Attack1()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("strike");
            playerMana.value -= 1;
            audioManager.Instance.PlaySFX("kiemchem");

        }


    }
    void Attack3()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            animator.SetTrigger("block");
            playerMana.value -= 1;
            audioManager.Instance.PlaySFX("skill2");

        }
    }
    public void buttonattack1()
    {
        if (playerMana.value > 0)
        {
            animator.SetTrigger("strike");
            playerMana.value -= 1;
            audioManager.Instance.PlaySFX("kiemchem");
        }
        else
        {
            Debug.Log("Không đủ mana để sử dụng kĩ năng!");
        }
    }
    public void buttonattack2()
    {
        if (playerMana.value > 0 && Time.time - lastCastTime >= castCooldown)
        {
            animator.SetTrigger("cast");
            playerMana.value -= 2;
            lastCastTime = Time.time;
            audioManager.Instance.PlaySFX("bandan");

            // Làm mờ button và chạy cooldown
            StartCoroutine(DisableButtonForCooldown(buttonImages[2]));

        }
        else
        {
            //  Debug.Log("Không đủ mana để sử dụng kĩ năng!");
        }
    }

    IEnumerator DisableButtonForCooldown(Image button)
    {
        if (button == null)
        {
            Debug.LogWarning("Button image không được gán!");
            yield break;
        }

        // Tìm Button component từ Image
        Button buttonComponent = button.GetComponent<Button>();
        if (buttonComponent == null)
        {
            Debug.LogWarning("Không tìm thấy Button component trên Image!");
            yield break;
        }
        skill3mo.SetActive(true);
        skill3.SetActive(false);
        // Tắt button và làm mờ
        Color color = button.color;
        color.a = 0.5f;
        button.color = color;
        buttonComponent.interactable = false;

        // Đợi 2 giây
        yield return new WaitForSeconds(2f);
        skill3mo.SetActive(false);
        skill3.SetActive(true);
        // Bật lại button và làm sáng
        color.a = 1f;
        button.color = color;
        buttonComponent.interactable = true;

        Debug.Log("Button đã trở lại bình thường sau 2 giây");
    }

    public void buttonattack3()
    {
        if (playerMana.value > 0)
        {
            animator.SetTrigger("block");
            playerMana.value -= 1;
            audioManager.Instance.PlaySFX("skill2");
        }
        else
        {
            Debug.Log("Không đủ mana để sử dụng kĩ năng!");
        }
    }
    private void UpdateSwordColliderPosition()
    {
        // Kiểm tra hướng nhân vật
        if (facingRight)
        {
            // Nếu hướng phải, đặt swordCollider bên phải
            swordCollider.transform.localPosition = new Vector3(0.2f, swordCollider.transform.localPosition.y, swordCollider.transform.localPosition.z);
            swordCollider.transform.localScale = new Vector3(1, 1, 1); // Điều chỉnh hướng Box Collider cho đúng
            swordCollider1.transform.localPosition = new Vector3(0.2f, swordCollider1.transform.localPosition.y, swordCollider1.transform.localPosition.z);
            swordCollider1.transform.localScale = new Vector3(1, 1, 1); // Điều chỉnh hướng Box Collider cho đúng
            swordCollider2.transform.localPosition = new Vector3(0.2f, swordCollider2.transform.localPosition.y, swordCollider2.transform.localPosition.z);
            swordCollider2.transform.localScale = new Vector3(1, 1, 1); // Điều chỉnh hướng Box Collider cho đúng
        }
        else
        {
            // Nếu hướng trái, đặt swordCollider bên trái
            swordCollider.transform.localPosition = new Vector3(-0.2f, swordCollider.transform.localPosition.y, swordCollider.transform.localPosition.z);
            swordCollider.transform.localScale = new Vector3(-1, 1, 1); // Điều chỉnh hướng Box Collider cho đúng
                                                                        // Nếu hướng trái, đặt swordCollider bên trái
            swordCollider1.transform.localPosition = new Vector3(-0.2f, swordCollider1.transform.localPosition.y, swordCollider1.transform.localPosition.z);
            swordCollider1.transform.localScale = new Vector3(-1, 1, 1); // Điều chỉnh hướng Box Collider cho đúng
                                                                         // Nếu hướng trái, đặt swordCollider bên trái
            swordCollider2.transform.localPosition = new Vector3(-0.2f, swordCollider2.transform.localPosition.y, swordCollider2.transform.localPosition.z);
            swordCollider2.transform.localScale = new Vector3(-1, 1, 1); // Điều chỉnh hướng Box Collider cho đúng
        }
    }



    public void ShowSword()
    {
        swordCollider.SetActive(true);

        UpdateSwordColliderPosition();
        swordCollider1.SetActive(false);
        swordCollider2.SetActive(false);

    }

    public void HideSword()
    {
        swordCollider.SetActive(false);
        swordCollider1.SetActive(false);
        swordCollider2.SetActive(false);
    }
    public void ShowSword1()
    {
        swordCollider1.SetActive(true);
        UpdateSwordColliderPosition();
        swordCollider.SetActive(false);
        swordCollider2.SetActive(false);
    }

    public void HideSword1()
    {
        swordCollider1.SetActive(false);
        swordCollider.SetActive(false);
        swordCollider2.SetActive(false);
    }
    public void ShowSword2()
    {
        swordCollider2.SetActive(true);
        UpdateSwordColliderPosition();
        swordCollider.SetActive(false);
        swordCollider1.SetActive(false);
    }

    public void HideSword2()
    {
        swordCollider2.SetActive(false);
        swordCollider.SetActive(false);
        swordCollider1.SetActive(false);
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
            facingRight = false;
            UpdateSwordColliderPosition();
        }
        else if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
            facingRight = true;
            UpdateSwordColliderPosition();
        }

        animator.SetBool("isWalking", moveInput != 0);
        animator.SetBool("IsGround", isGrounded);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetTrigger("Jump");
            //  isColliderActive = false;
            //if (boxCollider2D != null)
            //{
            //    boxCollider2D.enabled = false;
            //}
            audioManager.Instance.PlaySFX("jump");
        }
    }

    void Cast()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time - lastCastTime >= castCooldown)
        {
            animator.SetTrigger("cast");
            playerMana.value -= 2;
            lastCastTime = Time.time;
            audioManager.Instance.PlaySFX("bandan");
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

        if (collision.gameObject.CompareTag("ground1"))
        {

            isGrounded = true;
            Debug.Log("Chạm đất!");
            animator.SetBool("IsGround", true);
            //isColliderActive=true;
            //    if (boxCollider2D != null)
            //    {
            //        boxCollider2D.enabled = true;
            //    }
          
            isOnGround1 = true;



        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsGround", false);

        }
        if (collision.gameObject.CompareTag("ground1"))
        {
            isGrounded = false;
            animator.SetBool("IsGround", false);
            isOnGround1 = false;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("cauthang")) // Nếu va chạm với thang
        {
            isClimbing = true;
            Debug.Log("Nhân vật bắt đầu leo thang.");
        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("trap"))
        {
            animator.SetTrigger("dizzy");
            audioManager.Instance.PlaySFX("bidau");

        }

        if (collision.gameObject.CompareTag("vukhi_enemy"))
        {
            animator.SetTrigger("hurt");
            audioManager.Instance.PlaySFX("bidau");
            playerHealth.value -= 1;
            print("cham quai, -1 mau");
            if (playerHealth.value < 8)
            {
                fillImage.color = Color.yellow;

                if (playerHealth.value < 4)
                {
                    fillImage.color = Color.red;

                }
                if (playerHealth.value == 0)
                {
                    animator.SetTrigger("die");
                    StartCoroutine(WaitForDeathAnimation());
                    audioManager.Instance.PlaySFX("chet");
                    //    audioSource.PlayOneShot(audioClips[1]);
                    // Time.timeScale = 0;
                };
            }
            //   StartCoroutine(WaitForDeathAnimation());
        }
        if (collision.gameObject.CompareTag("ngonlua"))
        {
            StartCoroutine(DestroyTorchAfterDelay(collision.gameObject));
            gameManager.AddScore();
            gameManager.SetScoreText();
            torchCount++;
            audioManager.Instance.PlaySFX("anlua");
            Debug.Log("Bạn vừa nhận được ngọn đuốc");
            // Gọi hàm trong CotDuocManager để bật panel settings
            CotDuocManager cotDuocManager = FindObjectOfType<CotDuocManager>();
            if (cotDuocManager != null)
            {
                cotDuocManager.ActivatePanel();  // Bật panel settings
            }

        }

        if (collision.gameObject.CompareTag("quaman"))
        {
            SceneManager.LoadSceneAsync(2);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("cauthang")) // Nếu rời khỏi thang
        {
            isClimbing = false;
            Debug.Log("Nhân vật rời khỏi thang.");
        }
    }

    IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
    }
    IEnumerator DestroyTorchAfterDelay(GameObject torch)
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(torch);
    }







    public void tatnhacn()
    {
        tatnhacnen.SetActive(true);
        batnhacnen.SetActive(false);
        audioManager.ToggleMusic();


    }
    public void batnhacn()
    {
        tatnhacnen.SetActive(false);
        batnhacnen.SetActive(true);
        audioManager.ToggleMusic();
    }
    public void batamthanhok()
    {
        tatamthanh.SetActive(false);
        batamthanh.SetActive(true);
        audioManager.ToggleSFX();

    }
    public void tatamthanhok()
    {
        tatamthanh.SetActive(true);
        batamthanh.SetActive(false);
        audioManager.ToggleSFX();

    }
}