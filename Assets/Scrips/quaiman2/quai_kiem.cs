using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quai_kiem : MonoBehaviour
{
    public Transform pointA; // Điểm A
    public Transform pointB; // Điểm B
    public float speed = 2f; // Tốc độ di chuyển
    public float attackRange = 2f; // Phạm vi tấn công
    private Transform targetPoint; // Điểm mục tiêu
    public Transform player; // Nhân vật
    private bool isAttacking = false; // Quái có đang tấn công không
    private bool isDead = false; // Trạng thái quái đã chết
    private Animator animator; // Animator của quái
    public float yRange = 1f; // Phạm vi cho phép trên trục Y (có thể điều chỉnh)

    void Start()
    {
        targetPoint = pointB; // Ban đầu hướng đến điểm B
        player = GameObject.FindGameObjectWithTag("Player").transform; // Tìm nhân vật
        animator = GetComponent<Animator>(); // Lấy Animator từ quái
    }

    void Update()
    {
        if (isDead) return; // Nếu quái đã chết, dừng mọi hoạt động khác

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
                Attack(); // Tấn công
            }
        }
        else
        {
            // Kiểm tra xem player có trong phạm vi giữa A và B không và nằm trong phạm vi trục Y
            if (IsPlayerInPatrolRange())
            {
                // Nếu player ở trong phạm vi giữa A và B, tiến về phía player
                MoveTowardsPlayer();
            }
            else
            {
                // Di chuyển giữa các điểm A và B nếu player không trong phạm vi
                MoveBetweenPoints();
            }

            // Kiểm tra nếu player vào phạm vi tấn công
            if (Vector2.Distance(transform.position, player.position) < attackRange)
            {
                isAttacking = true;
                animator.SetBool("attack", true); // Bật hiệu ứng tấn công
            }
            else
            {
                animator.SetBool("attack", false); // Ngừng hiệu ứng tấn công
            }
        }
    }

    void MoveBetweenPoints()
    {
        // Di chuyển quái tới targetPoint
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

    void MoveTowardsPlayer()
    {
        // Di chuyển quái về phía player, chỉ xét trên trục X
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y); // Duy trì trục Y của quái
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        animator.SetBool("isMoving", true); // Bật hiệu ứng di chuyển

        // Quay đầu theo hướng di chuyển (chỉ xét trên trục X)
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1); // Quái quay sang trái
        else
            transform.localScale = new Vector3(1, 1, 1); // Quái quay sang phải
    }

    private void Attack()
    {
        // Logic tấn công có thể thêm vào đây, ví dụ như giảm máu của player khi quái tấn công
        Debug.Log("Boss is attacking!");

        // Bật animation tấn công
        animator.SetTrigger("attack");
    }

    private void FacePlayer()
    {
        // Quay mặt về phía player khi tấn công
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    private bool IsPlayerInPatrolRange()
    {
        // Kiểm tra xem player có trong phạm vi giữa pointA và pointB không và y của player phải gần y của quái
        bool isInXRange = player.position.x >= Mathf.Min(pointA.position.x, pointB.position.x) && player.position.x <= Mathf.Max(pointA.position.x, pointB.position.x);
        bool isInYRange = Mathf.Abs(player.position.y - transform.position.y) <= yRange; // Kiểm tra khoảng cách trên trục Y

        return isInXRange && isInYRange;
    }

    public void Die()
    {
        if (isDead) return; // Nếu quái đã chết thì không cần chạy lại hàm này

        isDead = true; // Đánh dấu là quái đã chết
        animator.SetBool("isMoving", false);
        animator.SetBool("attack", false);
        animator.SetTrigger("die"); // Kích hoạt animation chết
        // Thêm logic xử lý khi quái chết tại đây (vd: hủy object sau vài giây)
    }

    private void OnDisable()
    {
        // Đảm bảo tắt mọi animation khi quái ngừng hoạt động
        animator.SetBool("isMoving", false);
        animator.SetBool("attack", false);
    }
}
