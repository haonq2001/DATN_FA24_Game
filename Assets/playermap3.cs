using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class playermap3 : MonoBehaviour
{


    public GameObject skill3;
    public GameObject skill3mo;

    public gamemanagermap3 gameManager;

    public Slider playerHealth;
    public Image fillImage;
    public Slider playerMana;
    public Image fillImagemana;
    public GameObject boxattack;
    public GameObject boxskill2;
    public GameObject boxskill3;



    public float moveSpeed = 15f;

    public float moveNgang;
    public float moveDoc;
    public Vector2 movement;
    public Joystick joystick;



    // am thanh
    public List<AudioClip> audioClips;
    private AudioSource audioSource;

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

    public int torchCount = 0;  // Biến để lưu trữ số ngọn lửa (hoặc đuốc) của người chơi
    public int health = 10;
    public int mana = 10;


    public Image[] buttonImages; // Biến để tham chiếu đến Image của button


    private float jumpCooldown = 0.3f;  // Khoảng thời gian giữa các lần nhảy
    private float lastJumpTime = 0f;    // Thời gian nhảy lần cuối
    void Start()
    {











        //  boxCollider2D = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Gán Animator từ component

        skill3mo.SetActive(false);
        skill3.SetActive(true);

        boxattack.SetActive(false);
        boxskill2.SetActive(false);
        boxskill3.SetActive(false);



        playerHealth.interactable = false;
        playerMana.interactable = false;
        playerHealth.maxValue = health;

        playerMana.maxValue = mana;
        playerHealth.value = Mathf.Clamp(health, 0, playerHealth.maxValue);
        playerMana.value = Mathf.Clamp(mana, 0, playerMana.maxValue);

        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1;
    }
    // bh fix loi xong them chu d vao


    void Update()
    {
        Move();
        Jump();
        Attack();



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
            boxattack.SetActive(true);
        }
    }
    public void buttonattack()
    {
        animator.SetTrigger("attack");
        audioManager.Instance.PlaySFX("skill1");
        boxattack.SetActive(true);
    }

    void Attack1()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("strike");
            playerMana.value -= 1;
            audioManager.Instance.PlaySFX("kiemchem");
            boxskill2.SetActive(true);
            PlayerPrefs.SetInt("PlayerMana", mana);

        }


    }
    void Attack3()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            animator.SetTrigger("block");
            playerMana.value -= 1;
            audioManager.Instance.PlaySFX("skill2");
            boxskill3.SetActive(true);
            PlayerPrefs.SetInt("PlayerMana", mana);

        }
    }
    public void buttonattack1()
    {
        if (playerMana.value > 0)
        {
            animator.SetTrigger("strike");
            playerMana.value -= 1;
            audioManager.Instance.PlaySFX("kiemchem");
            boxskill2.SetActive(true);
            PlayerPrefs.SetInt("PlayerMana", mana);
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
            PlayerPrefs.SetInt("PlayerMana", mana);

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
            PlayerPrefs.SetInt("PlayerMana", mana);
            audioManager.Instance.PlaySFX("skill2");
            boxskill3.SetActive(true);
        }
        else
        {
            Debug.Log("Không đủ mana để sử dụng kĩ năng!");
        }
    }
    private void UpdateSwordColliderPosition()
    {
        // Đổi hướng dựa trên `facingRight`
        if (facingRight)
        {
            // Hướng sang phải
            boxattack.transform.localPosition = new Vector3(1.0f, boxattack.transform.localPosition.y, boxattack.transform.localPosition.z);
            boxattack.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // Scale mặc định

            boxskill2.transform.localPosition = new Vector3(1.0f, boxskill2.transform.localPosition.y, boxskill2.transform.localPosition.z);
            boxskill2.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // Scale mặc định

            boxskill3.transform.localPosition = new Vector3(1.0f, boxskill3.transform.localPosition.y, boxskill3.transform.localPosition.z);
            boxskill3.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // Scale mặc định
        }
        else
        {
            // Hướng sang trái
            boxattack.transform.localPosition = new Vector3(-1.0f, boxattack.transform.localPosition.y, boxattack.transform.localPosition.z);
            boxattack.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f); // Lật đối tượng theo trục X
                                                                             // Có thể lật góc của collider nếu cần
                                                                             // boxattack.transform.localRotation = Quaternion.Euler(0, 180, 0); // Quay boxattack 180 độ trên trục Y

            boxskill2.transform.localPosition = new Vector3(-1.0f, boxskill2.transform.localPosition.y, boxskill2.transform.localPosition.z);
            boxskill2.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

            boxskill3.transform.localPosition = new Vector3(-1.0f, boxskill3.transform.localPosition.y, boxskill3.transform.localPosition.z);
            boxskill3.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
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
                UpdateSwordColliderPosition();
            }
            else if (moveInput > 0)
            {
                spriteRenderer.flipX = false;
                facingRight = true;
                UpdateSwordColliderPosition();
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
        // Chỉ cho phép nhảy nếu thời gian từ lần nhảy trước đã đủ lâu
        if ((joystick.Vertical > 0.5f || Input.GetButtonDown("Jump")) && Time.time - lastJumpTime >= jumpCooldown)
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;  // Đánh dấu là không còn chạm đất
                animator.SetTrigger("Jump");
                audioManager.Instance.PlaySFX("jump");
                lastJumpTime = Time.time;  // Cập nhật thời gian nhảy
            }
        }
    }

    void Cast()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time - lastCastTime >= castCooldown)
        {
            animator.SetTrigger("cast");
            playerMana.value -= 2;
            PlayerPrefs.SetInt("PlayerMana", mana);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("trap"))
        {
            animator.SetTrigger("dizzy");
            audioManager.Instance.PlaySFX("bidau");
            PlayerPrefs.SetInt("PlayerHealth", health);
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
                    ;
                };
            }

        }

        if (collision.gameObject.CompareTag("vukhi_enemy"))
        {
            if (boxskill3.activeSelf)
            {
                animator.SetTrigger("hurt");
                audioManager.Instance.PlaySFX("bidau");
                playerHealth.value -= 0;
                PlayerPrefs.SetInt("PlayerHealth", health);

                print("the phong thu khong mat mau");
            }
            else
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
                        ;
                    };
                }

            }
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

    IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
        gameManager.GameOver();
        PlayerPrefs.DeleteAll(); // Xóa dữ liệu lưu
    }
    IEnumerator DestroyTorchAfterDelay(GameObject torch)
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(torch);
    }








}