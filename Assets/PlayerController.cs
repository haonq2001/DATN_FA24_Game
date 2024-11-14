using System.Collections;
using System.Collections.Generic;
using UnityEngine;



using UnityEngine.SceneManagement;
using UnityEngine.UI;





public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    public Slider playerHealth;
    public Image fillImage;
    public Slider playerMana;
    public Image fillImagemana;



    public float moveSpeed = 2f;
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





    public int torchCount = 0;  // Biến để lưu trữ số ngọn lửa (hoặc đuốc) của người chơi


    public Image[] buttonImages; // Biến để tham chiếu đến Image của button




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        swordCollider.SetActive(false);
        swordCollider1.SetActive(false);


        playerHealth.maxValue = health;
        playerHealth.value = health;
        playerMana.maxValue = mana;
        playerMana.value = mana;


    }

    void Update()
    {
        Move();
        Jump();
        Cast();
        Attack();


        if (playerMana.value > 0) {
            Attack1();
            Cast();
            Attack3();
        }
        else
        {
            Debug.Log("Không đủ năng lượng để sử dụng kĩ năng");
        }

        foreach (Image buttonImage in buttonImages)
        {
            UpdateButton(buttonImage);
        }
    }

    void UpdateButton(Image button)
    {
        if (playerMana.value <= 0)
        {
            Color color = button.color;
            color.a = 0.5f;
            button.color = color;
            button.GetComponent<Button>().interactable = false;
        }
        else
        {
            Color color = button.color;
            color.a = 1f;
            button.color = color;
            button.GetComponent<Button>().interactable = true;
        }
    }


        Attack1();

        // Kiểm tra nhấn phím Space để bắn đạn
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    PlayerShoot();
        //}
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
        }
    }

    void Attack1()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("strike");

            playerMana.value -= 1;
        }
    }

    void Attack3()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            animator.SetTrigger("block");
            playerMana.value -= 1;
        }
    }

    public void buttonattack1()
    {
        if (playerMana.value > 0)
        {
            animator.SetTrigger("strike");
            playerMana.value -= 1;
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
            StartCoroutine(DisableButtonForCooldown(buttonImages[2]));
        }
    }

    IEnumerator DisableButtonForCooldown(Image button)
    {
        if (button == null)
        {
            Debug.LogWarning("Button image không được gán!");
            yield break;
        }

        Button buttonComponent = button.GetComponent<Button>();
        if (buttonComponent == null)
        {
            Debug.LogWarning("Không tìm thấy Button component trên Image!");
            yield break;
        }

        Color color = button.color;
        color.a = 0.5f;
        button.color = color;
        buttonComponent.interactable = false;

        yield return new WaitForSeconds(2f);

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
        }
        else
        {
            Debug.Log("Không đủ mana để sử dụng kĩ năng!");
        }
    }


        }
    }


    private void UpdateSwordColliderPosition()
    {
        if (facingRight)
        {
            swordCollider.transform.localPosition = new Vector3(0.2f, swordCollider.transform.localPosition.y, swordCollider.transform.localPosition.z);
            swordCollider.transform.localScale = new Vector3(1, 1, 1);
            swordCollider1.transform.localPosition = new Vector3(0.2f, swordCollider.transform.localPosition.y, swordCollider.transform.localPosition.z);
            swordCollider1.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            swordCollider.transform.localPosition = new Vector3(-0.2f, swordCollider.transform.localPosition.y, swordCollider.transform.localPosition.z);
            swordCollider.transform.localScale = new Vector3(-1, 1, 1);
            swordCollider1.transform.localPosition = new Vector3(-0.2f, swordCollider.transform.localPosition.y, swordCollider.transform.localPosition.z);
            swordCollider1.transform.localScale = new Vector3(-1, 1, 1);
        }
    }


    public void ShowSword()
    {
        swordCollider.SetActive(true);
        UpdateSwordColliderPosition();
        swordCollider1.SetActive(false);



    public void ShowSword()
    {
        swordCollider.SetActive(true);

    }

    public void HideSword()
    {
        swordCollider.SetActive(false);
    }

    public void ShowSword1()
    {
        swordCollider1.SetActive(true);
    }

    public void HideSword1()
    {
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
        }
    }

    void Cast()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time - lastCastTime >= castCooldown)
        {
            animator.SetTrigger("cast");
            lastCastTime = Time.time;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("bay"))
        {
            animator.SetTrigger("die");
            StartCoroutine(WaitAndStop());
        }

        if (collision.gameObject.CompareTag("Torch"))
        {

            torchCount++;


            Destroy(collision.gameObject);
        }
    }

    private IEnumerator WaitAndStop()
    {
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0;
    }






    // public void TakeDamage(int damage)
    // {
    // 	health -= damage;

    // 	StartCoroutine(DamageAnimation());

    // 	if (health <= 0)
    // 	{
    // 		// Die();
    // 	}
    // }

    // void Die()
    // {
    //     Time.timeScale = 0;
    // }
    // IEnumerator DamageAnimation()
    // {
    // 	SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

    // 	for (int i = 0; i < 3; i++)
    // 	{
    // 		foreach (SpriteRenderer sr in srs)
    // 		{
    // 			Color c = sr.color;
    // 			c.a = 0;
    // 			sr.color = c;
    // 		}

    // 		yield return new WaitForSeconds(.1f);

    // 		foreach (SpriteRenderer sr in srs)
    // 		{
    // 			Color c = sr.color;
    // 			c.a = 1;
    // 			sr.color = c;
    // 		}

    // 		yield return new WaitForSeconds(.1f);
    // 	}
    // }

}
