using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{


    private bool hasBeenAttacked = false;
    public GameObject boxvukhi; // Reference to the weapon box
    public GameObject thanhmau;

    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float attackRange = 3f;
    private Transform targetPoint;
    public Transform player;
    private bool isAttacking = false;
    private Animator animator;

    public Slider BossHealth;
    public Image fillImage;
    public float health = 10;

    public GameObject torchPrefab;  // Reference to the torch object
    public Transform dropPoint;

    // Point where the torch will drop
    private AudioSource audioSource;


    public int enemyIndex; // Chỉ số duy nhất của quái
    void Start()
    {
        targetPoint = pointB; // Initially move towards point B
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>(); // Get the Animator component
        BossHealth.maxValue = health;
        BossHealth.value = health;
        BossHealth.interactable = false;

        boxvukhi.SetActive(false); // Initially hide the weapon box

        audioSource = GetComponent<AudioSource>();

        // Kiểm tra xem có dữ liệu máu quái đã lưu chưa
        if (PlayerPrefs.HasKey("EnemyHealth" + enemyIndex))
        {
            BossHealth.value = PlayerPrefs.GetFloat("EnemyHealth" + enemyIndex); // Tải lại giá trị máu
        }
        else
        {
            BossHealth.value = health; // Nếu không có dữ liệu, khởi tạo lại giá trị máu mặc định
        }

        BossHealth.maxValue = health;
        BossHealth.interactable = false;
        fillImage.color = Color.green; // Đặt màu mặc định cho thanh máu

        // Kiểm tra trạng thái màu sắc đã lưu
        if (PlayerPrefs.HasKey("HasBeenAttacked") && PlayerPrefs.GetInt("HasBeenAttacked") == 1)
        {
            fillImage.color = Color.red; // Nếu đã bị tấn công và màu vàng đã được lưu, áp dụng màu vàng
        }
        else
        {
            fillImage.color = Color.red; // Nếu chưa bị tấn công, giữ màu xanh
        }
    }


    public void vukhion()
    {
        boxvukhi.SetActive(true); // Show the weapon box
    }

    public void vukhioff()
    {
        boxvukhi.SetActive(false); // Hide the weapon box
    }

    void Update()
    {
        if (isAttacking)
        {
            // Check if the player is out of attack range
            if (Vector2.Distance(transform.position, player.position) > attackRange)
            {
                isAttacking = false;
                animator.SetBool("attack", false); // Stop attack animation
                targetPoint = Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position) ? pointB : pointA;
            }
            else
            {
                FacePlayer(); // Face towards the player
                Attack();
            }
        }
        else
        {
            // Move between points and enable movement animation
            MoveBetweenPoints();
            if (Vector2.Distance(transform.position, player.position) < attackRange)
            {
                isAttacking = true;
                animator.SetBool("attack", true); // Start attack animation
            }
        }
    }

    void MoveBetweenPoints()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        animator.SetBool("isMoving", true); // Enable movement animation

        // Flip the boss based on movement direction
        if (targetPoint.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Face left
            MoveBoxCollider(-0.5f); // Move the box collider to the left
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1); // Face right
            MoveBoxCollider(0.5f); // Move the box collider to the right
        }

        // Change direction when reaching point A or B
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }
    }

    void MoveBoxCollider(float offset)
    {
        // Update the position of the box collider based on the boss's facing direction
        Vector3 boxPosition = boxvukhi.transform.localPosition;
        boxvukhi.transform.localPosition = new Vector3(offset, boxPosition.y, boxPosition.z);
    }

    private void Attack()
    {
        Debug.Log("Boss is attacking!");
        animator.SetBool("attack", true);
        // Add additional attack logic here if needed
    }

    private void FacePlayer()
    {
        // Face towards the player when attacking
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1); // Face left
        else
            transform.localScale = new Vector3(1, 1, 1); // Face right
    }

    private void OnDisable()
    {
        // Ensure all animations are stopped when the boss is disabled
        animator.SetBool("isMoving", false);
        animator.SetBool("attack", false);
    }


   private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Bullet") || collision.CompareTag("Sword"))
    {
        BossHealth.value -= 2; // Giảm máu của quái
        PlayerPrefs.SetFloat("EnemyHealth" + enemyIndex, BossHealth.value); // Lưu trạng thái thanh máu của quái
        PlayerPrefs.Save();

        animator.SetBool("hulk", true); // Animation bị tấn công
        audioManager.Instance.PlaySFX("matmau"); // Âm thanh bị trúng đòn

        // Cập nhật màu sắc thanh máu
        UpdateHealthUI();

        // Lưu trạng thái màu sắc nếu cần
        if (BossHealth.value < 8 && BossHealth.value >= 4)
        {
            PlayerPrefs.SetInt("HasBeenAttacked", 1);  // Lưu trạng thái bị tấn công (màu vàng)
            PlayerPrefs.Save();
        }

        if (BossHealth.value <= 0)
        {
            // Khi máu quái = 0, thực hiện hành động chết
            animator.SetTrigger("die");
            StartCoroutine(WaitForDeathAnimation()); // Chờ animation chết hoàn thành
        }
    }
}

    private void UpdateHealthUI()
    {
        if (BossHealth.value < 4)
        {
            fillImage.color = Color.red; // Nếu máu quá thấp, chuyển thanh máu sang màu đỏ
            hasBeenAttacked = true; // Đánh dấu rằng quái đã bị tấn công
        }
        else if (BossHealth.value < 8 && BossHealth.value >= 4)
        {
            // Nếu quái đã bị tấn công và máu đang ở mức trung bình, chuyển thanh máu sang màu vàng
            if (!hasBeenAttacked)
            {
                fillImage.color = Color.yellow;
            }
        }
        else
        {
            fillImage.color = Color.red; // Nếu máu đầy, thanh máu màu xanh
            hasBeenAttacked = false; // Đặt lại trạng thái khi máu đầy
        }
    }


    IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(1f);  // Chờ 1 giây để hoàn thành animation chết
        Die();  // Xử lý việc xóa quái
    }

    public void Die()
    {// Xử lý cái chết của quái
        gameObject.SetActive(false); // Ẩn quái vật
        Destroy(thanhmau); // Hủy thanh máu nếu có

        // Lưu trạng thái quái đã chết
        PlayerPrefs.SetInt("Enemy" + enemyIndex + "Dead", 1); // Lưu trạng thái đã chết
        PlayerPrefs.Save();

        // Xóa thanh máu khi quái chết
        PlayerPrefs.DeleteKey("EnemyHealth" + enemyIndex); // Xóa dữ liệu máu của quái khi chết

        // Drop các vật phẩm, như ngọn đuốc
        if (torchPrefab != null && dropPoint != null)
        {
            Instantiate(torchPrefab, dropPoint.position, Quaternion.identity);  // Drop ngọn đuốc tại vị trí dropPoint
        }

    }






}