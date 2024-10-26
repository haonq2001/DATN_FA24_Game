using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float attackRange = 2f;
    private Transform targetPoint;
    public Transform player;
    private bool isAttacking = false;
    private Animator animator;

    void Start()
    {
        targetPoint = pointB; // Ban đầu hướng đến điểm B
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>(); // Lấy Animator từ boss
    }

    void Update()
    {
        if (isAttacking)
        {
            // Kiểm tra nếu player đã ra khỏi phạm vi tấn công
            if (Vector2.Distance(transform.position, player.position) > attackRange)
            {
                isAttacking = false;
                animator.SetBool("attack", false); // Ngừng hiệu ứng tấn công
                targetPoint = Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position) ? pointB : pointA;
            }
            else
            {
                FacePlayer(); // Quay mặt về hướng player
                Attack();
            }
        }
        else
        {
            // Di chuyển giữa các điểm và bật animation di chuyển
            MoveBetweenPoints();
            if (Vector2.Distance(transform.position, player.position) < attackRange)
            {
                isAttacking = true;
                animator.SetBool("attack", true); // Bật hiệu ứng tấn công
            }
        }
    }

    void MoveBetweenPoints()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        animator.SetBool("isMoving", true); // Bật hiệu ứng di chuyển

        // Quay đầu theo hướng di chuyển
        if (targetPoint.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        // Đổi hướng khi tới điểm A hoặc B
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }
    }

    private void Attack()
    {
        Debug.Log("Boss is attacking!");
        animator.SetBool("attack", true);
        // Add additional attack logic here if needed
    }

    private void FacePlayer()
    {
        // Quay mặt về phía player khi tấn công
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnDisable()
    {
        // Đảm bảo tắt mọi animation khi boss ngừng hoạt động
        animator.SetBool("isMoving", false);
        animator.SetBool("attack", false);
    }
}
