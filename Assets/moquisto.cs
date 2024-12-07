using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MuoiDiChuyen : MonoBehaviour
{
    public GameObject boxvukhi;
    public GameObject thanhmau;
    public float startX = -5f; // Điểm đầu (trục X)
    public float endX = 5f; // Điểm cuối (trục X)
    public float maxY = 5f; // Chiều cao tối đa (trục Y)
    public float minY = 0f; // Chiều cao tối thiểu (trục Y)
    public float speed = 3f; // Tốc độ di chuyển
    public float waitTime = 1f; // Thời gian dừng ngẫu nhiên giữa các lần di chuyển
    public float raycastDistance = 5f; // Phạm vi tìm kiếm Player
    public LayerMask playerLayer; // Layer của Player
    public float attackCooldown = 1f; // Thời gian chờ giữa các lần tấn công

    public int maxHealth = 50; // Máu tối đa
    private int currentHealth; // Máu hiện tại
    public Slider BossHealth;
    public Image fillImage;

    private Vector3 targetPosition; // Vị trí mục tiêu
    private Animator animator; // Animator điều khiển animation
    private SpriteRenderer spriteRenderer; // SpriteRenderer để quay đầu
    private bool isDead = false; // Trạng thái chết
    private bool isAttacking = false; // Trạng thái tấn công

    private float checkInterval = 0.2f; // Thời gian giữa các lần kiểm tra
    private float lastCheckTime = 0f; // Thời điểm kiểm tra gần nhất

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        BossHealth.maxValue = currentHealth;
        BossHealth.value = currentHealth;
        BossHealth.interactable = false;


        StartCoroutine(RandomMovement());
    }

    private void Update()
    {
        if (isDead || isAttacking) return;

        if (Time.time - lastCheckTime >= checkInterval)
        {
            lastCheckTime = Time.time;
            CheckForPlayer();
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

    private IEnumerator RandomMovement()
    {
        while (!isDead)
        {
            targetPosition = GetRandomPosition();
            animator.SetBool("IsFlying", true);

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f && !isDead)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                spriteRenderer.flipX = direction.x < 0;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            animator.SetBool("IsFlying", false);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(startX, endX);
        float randomY = Random.Range(minY, maxY);
        return new Vector3(randomX, randomY, transform.position.z);
    }

    private void CheckForPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, raycastDistance, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            float distanceToPlayer = Vector3.Distance(transform.position, hit.transform.position);

            if (distanceToPlayer <= 1.5f && !isAttacking)
            {
                StopCoroutine(RandomMovement());
                StartCoroutine(AttackPlayer());
            }
            else if (distanceToPlayer > 1.5f)
            {
                MoveTowardsPlayer(hit.transform.position);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Sword"))
        {
            BossHealth.value -= 2;
            animator.SetTrigger("Hurt");
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

                animator.SetTrigger("Die");
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


private void MoveTowardsPlayer(Vector3 playerPosition)
    {
        Vector3 direction = (playerPosition - transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        animator.SetBool("IsFlying", true);
    }

    private IEnumerator AttackPlayer()
    {
        if (isAttacking || isDead) yield break;

        isAttacking = true;
        animator.SetBool("IsFlying", false);
        animator.SetTrigger("Attack");

        // Chờ thời gian tấn công và cooldown
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        StartCoroutine(RandomMovement());
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        StopAllCoroutines();
        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raycastDistance);
    }
}
