using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class khungLong_nho : MonoBehaviour
{
   public Transform pointA; // Điểm A
    public Transform pointB; // Điểm B
    public float speed = 2f; // Tốc độ di chuyển
    public float attackRange = 5f; // Phạm vi tấn công
    private Transform targetPoint; // Điểm hiện tại để di chuyển
    public Transform player; // Nhân vật
    private bool isAttacking = false; // Quái có đang tấn công không
    private bool isDead = false; // Trạng thái quái đã chết
    private Animator animator;

    public GameObject bulletPrefab; // Prefab của đạn
    public Transform firePoint; // Vị trí bắn đạn
    public float bulletSpeed = 5f; // Tốc độ đạn
    public float shootCooldown = 1f; // Thời gian chờ giữa các lần bắn
    private float lastShootTime = 0f; // Thời gian lần bắn gần nhất

    public Slider BossHealth;
    public Image fillImage;
    public float health = 10;
    // public GameObject torchPrefab;  // Reference to the torch object
    public Transform dropPoint;
    private AudioSource audioSource;
    public GameObject thanhmau;


    void Start()
    {
        targetPoint = pointB;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        BossHealth.maxValue = health;
        BossHealth.value = health;
        BossHealth.interactable = false;
    }

    void Update()
    {
        if (isDead) return;

        // Kiểm tra nếu player nằm trong tầm tấn công
        if (IsPlayerInRange())
        {
            isAttacking = true;
            MoveTowardsPlayer();

            // Nếu đã sẵn sàng bắn
            if (CanShoot())
            {
                // Shoot(); // Bắn đạn
                animator.SetBool("attack", true);
            }
        }
        else
        {
            isAttacking = false;
            MoveBetweenPoints(); // Di chuyển giữa điểm A và B
            animator.SetBool("attack", false); // Tắt animation tấn công
        }

        // Luôn bật animation di chuyển khi quái đang di chuyển
        if (!isAttacking)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            // animator.SetBool("isMoving", false); // Tắt animation di chuyển khi quái đang tấn công
        }
    }

    bool IsPlayerInRange()
    {
        // Kiểm tra khoảng cách từ quái đến nhân vật
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer <= attackRange;
    }

    bool CanShoot()
    {
        // Kiểm tra xem quái đã sẵn sàng bắn chưa
        return Time.time - lastShootTime >= shootCooldown;
    }

    public void Shoot()
    {
        // Tính vị trí giữa người của player
        Vector2 targetPosition = new Vector2(player.position.x, player.position.y - 0.7f); // Giả sử 0.5f là giữa người

        // Tính hướng bắn
        Vector2 shootDirection = (targetPosition - (Vector2)firePoint.position).normalized;

        // Tạo đạn và thiết lập hướng
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootDirection * bulletSpeed; // Đặt vận tốc cho viên đạn
        }
        else
        {
            Debug.LogError("Rigidbody2D is missing on the bullet prefab!");
        }

        // Bật animation attack
        animator.SetBool("attack", true);

        // Cập nhật thời gian bắn gần nhất
        lastShootTime = Time.time;
    }



    void MoveBetweenPoints()
    {
        // Kiểm tra nếu quái không nhận diện nhân vật trong phạm vi tấn công
        if (!IsPlayerInRange())
        {
            // Di chuyển giữa A và B nếu không có nhân vật trong phạm vi tấn công
            Vector2 newPosition = Vector2.MoveTowards(transform.position, new Vector2(targetPoint.position.x, transform.position.y), speed * Time.deltaTime);

            // Đảm bảo quái không vượt quá phạm vi giữa pointA và pointB
            newPosition.x = Mathf.Clamp(newPosition.x, pointA.position.x, pointB.position.x);

            // Cập nhật vị trí của quái
            transform.position = newPosition;

            // Quay mặt theo hướng di chuyển
            if (targetPoint.position.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            // Kiểm tra nếu quái đã đến gần điểm hiện tại (pointA hoặc pointB)
            if (Mathf.Abs(transform.position.x - targetPoint.position.x) < 0.1f)
            {
                // Đổi mục tiêu giữa pointA và pointB khi quái đến gần
                targetPoint = targetPoint == pointA ? pointB : pointA;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Kiểm tra nếu nhân vật trong phạm vi tấn công
        if (IsPlayerInRange())
        {
            Vector2 direction = (player.position - transform.position).normalized;

            // Tính vị trí mới trên trục X và giữ nguyên Y
            Vector2 newPosition = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), speed * Time.deltaTime);

            // Đảm bảo quái không vượt quá phạm vi giữa pointA và pointB
            newPosition.x = Mathf.Clamp(newPosition.x, pointA.position.x, pointB.position.x);

            // Cập nhật vị trí của quái
            transform.position = newPosition;

            // Quay mặt về hướng di chuyển
            transform.localScale = new Vector3(direction.x < 0 ? -1 : 1, 1, 1);
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("isMoving", false);
        animator.SetTrigger("die");
    }

    private void OnDisable()
    {
        animator.SetBool("isMoving", false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Sword"))
        {
            BossHealth.value -= 2;
            animator.SetTrigger("hulk");
           // audioManager.Instance.PlaySFX("matmau");
            if (BossHealth.value < 8)
            {
                fillImage.color = Color.yellow;
            }
            if (BossHealth.value < 4)
            {
                fillImage.color = Color.red;
            }
            if (BossHealth.value <= 0)
            {
                // Drop the torch

                animator.SetTrigger("die");
                StartCoroutine(WaitForDeathAnimation());


            }
        }
    }
    IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);  // Destroy the boss
        Destroy(thanhmau);
        // if (torchPrefab != null && dropPoint != null)
        // {
        //     Instantiate(torchPrefab, dropPoint.position, Quaternion.identity);  // Drop the torch at dropPoint
        // }
    }
}