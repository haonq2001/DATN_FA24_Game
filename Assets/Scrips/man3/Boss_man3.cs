using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_man3 : MonoBehaviour
{
    public Transform pointA; // Điểm A
    public Transform pointB; // Điểm B
    public float speed = 2f; // Tốc độ di chuyển
    public float attackRange = 5f; // Phạm vi tấn công chiêu 1
    public float attack2Range = 8f; // Phạm vi tấn công chiêu 2
    public float maxHeightDifference = 3f; // Phạm vi nhận diện theo chiều dọc
    public float attackCooldown = 3f; // Khoảng cách giữa các lần tấn công
    public GameObject lightningPrefab; // Prefab tia sét
    public Transform firePoint; // Vị trí bắn tia sét
    public float lightningSpeed = 5f; // Tốc độ tia sét

    private Transform player; // Nhân vật
    private Transform targetPoint; // Điểm hiện tại để di chuyển
    private bool isAttacking = false; // Boss đang tấn công
    private bool isDead = false; // Boss đã chết
    private int attackCounter = 0; // Đếm số lần chém (Attack1)
    private float lastAttackTime = -999f; // Thời gian lần tấn công gần nhất
    private Animator animator;

    void Start()
    {
        targetPoint = pointB; // Điểm khởi tạo là pointB
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        if (IsPlayerInDetectionRange())
        {
            MoveTowardsPlayer();

            // Tấn công khi đủ thời gian
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                if (attackCounter < 2 && IsPlayerInAttack1Range())
                {
                    StartAttack1(); // Thực hiện Attack1
                }
                else if (attackCounter >= 2 && IsPlayerInAttack2Range())
                {
                    StartAttack2(); // Thực hiện Attack2
                }
            }
        }
        else
        {
            MoveBetweenPoints();
        }

        animator.SetBool("isWalking", !isAttacking);
        animator.SetBool("isRunning", IsPlayerInDetectionRange() && !isAttacking);
    }

    bool IsPlayerInDetectionRange()
    {
        bool isWithinHorizontalRange = player.position.x >= pointA.position.x && player.position.x <= pointB.position.x;
        float distanceY = Mathf.Abs(transform.position.y - player.position.y);
        bool isWithinVerticalRange = distanceY <= maxHeightDifference;

        return isWithinHorizontalRange && isWithinVerticalRange;
    }

    bool IsPlayerInAttack1Range()
    {
        float distanceX = Mathf.Abs(transform.position.x - player.position.x);
        return distanceX <= attackRange;
    }

    bool IsPlayerInAttack2Range()
    {
        float distanceX = Mathf.Abs(transform.position.x - player.position.x);
        return distanceX <= attack2Range; // Tấn công trong cả phạm vi attackRange và attack2Range
    }

    void MoveTowardsPlayer()
    {
        if (!isAttacking)
        {
            float distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);

            if (distanceToPlayer > attackRange)
            {
                Vector2 direction = new Vector2(player.position.x - transform.position.x, 0).normalized;
                Vector2 targetPosition = new Vector2(player.position.x, transform.position.y) - direction * attackRange;

                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                transform.localScale = new Vector3(direction.x < 0 ? -1 : 1, 1, 1);
            }
        }
    }

    void MoveBetweenPoints()
    {
        if (!isAttacking)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, new Vector2(targetPoint.position.x, transform.position.y), speed * Time.deltaTime);
            transform.position = newPosition;

            if (Mathf.Abs(transform.position.x - targetPoint.position.x) < 0.1f)
            {
                targetPoint = targetPoint == pointA ? pointB : pointA;
            }

            transform.localScale = new Vector3(targetPoint.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
    }

    void StartAttack1()
    {
        isAttacking = true;
        animator.SetBool("Attack1", true);
        attackCounter++; // Tăng bộ đếm Attack1
        lastAttackTime = Time.time;
        StartCoroutine(ResetAttackAnimation("Attack1"));
        StartCoroutine(EndAttack());
    }

    void StartAttack2()
    {
        isAttacking = true;
        animator.SetBool("Attack2", true);
        attackCounter = 0; // Reset bộ đếm sau khi dùng Attack2
        lastAttackTime = Time.time;
        StartCoroutine(PerformAttack2());
    }

    IEnumerator PerformAttack2()
    {
        yield return new WaitForSeconds(0.5f); // Đợi animation hoàn thành trước khi bắn
        ShootLightning();
        yield return new WaitForSeconds(0.5f); // Đợi phần còn lại của animation
        StartCoroutine(ResetAttackAnimation("Attack2"));
        StartCoroutine(EndAttack());
    }

    void ShootLightning()
    {
        if (firePoint != null && lightningPrefab != null)
        {
            Vector2 shootDirection = (player.position - firePoint.position).normalized;

            GameObject lightning = Instantiate(lightningPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = lightning.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = shootDirection * lightningSpeed;
            }
        }
    }

    IEnumerator ResetAttackAnimation(string attackParameter)
    {
        yield return new WaitForSeconds(1f); // Thời gian chạy animation
        animator.SetBool(attackParameter, false);
    }

    IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(1f); // Đợi animation kết thúc
        isAttacking = false;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        isAttacking = false;
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetTrigger("Die");
    }

    private void OnDisable()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }
}
