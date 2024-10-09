using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playcontroller : MonoBehaviour
{
    [SerializeField] private int m_speed = 4;
    [SerializeField] private int m_jumpForce = 7; // Lực nhảy
    [SerializeField] private int m_rollForce = 6;
    [SerializeField] private bool m_noBlood = false;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private PlayerMove m_groundSensor;
    private PlayerMove m_wallSensorR1;
    private PlayerMove m_wallSensorR2;
    private PlayerMove m_wallSensorL1;
    private PlayerMove m_wallSensorL2;

    private bool m_isWallSliding = false;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private bool m_isJumping = false; // Theo dõi trạng thái nhảy
    private int m_facingDirection = 1; // 1 = phải, -1 = trái
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;

    [SerializeField] private GameObject m_slideDust;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<PlayerMove>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<PlayerMove>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<PlayerMove>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<PlayerMove>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<PlayerMove>();
    }

    void Update()
    {
        m_timeSinceAttack += Time.deltaTime;
        HandleGroundedState();
        HandleMovement();
        HandleActions();
    }

    private void HandleGroundedState()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);
        if (hit.collider != null)
        {
            if (!m_grounded)
            {
                m_grounded = true;
                m_animator.SetBool("Grounded", m_grounded);
                m_isJumping = false; // Đặt trạng thái nhảy về false khi tiếp đất
                m_animator.SetInteger("AnimState", 0); // Chuyển sang trạng thái idle
                Debug.Log("Nhân vật đã tiếp đất.");
            }
        }
        else
        {
            if (m_grounded)
            {
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                Debug.Log("Nhân vật đã rời khỏi mặt đất.");
            }
        }
    }

    private void HandleMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        // Cập nhật AirSpeedY dựa trên tốc độ hiện tại
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // Lật sprite dựa trên hướng di chuyển
        if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true; // Quay trái
            m_facingDirection = -1;
        }
        else if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false; // Quay phải
            m_facingDirection = 1;
        }

        // Xử lý trạng thái lăn
        if (m_rolling)
        {
            m_rollCurrentTime += Time.deltaTime;
            if (m_rollCurrentTime > m_rollDuration)
                m_rolling = false;
        }
    }

    private void HandleActions()
    {
        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
        else if (Input.GetKeyDown("q") && !m_rolling)
        {
            m_animator.SetTrigger("Hurt");
        }
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            PerformAttack();
        }
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            m_animator.SetBool("IdleBlock", false);
        }
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            PerformRoll();
        }
        else if (Input.GetKeyDown("space") && m_grounded) // Kiểm tra nhảy
        {
            PerformJump();
        }
        else
        {
            HandleIdleState();
        }
    }

    private void PerformAttack()
    {
        m_currentAttack++;
        if (m_currentAttack > 4)
            m_currentAttack = 1;

        if (m_timeSinceAttack > 1.0f)
            m_currentAttack = 1;

        m_animator.SetTrigger("Attack" + m_currentAttack);
        m_timeSinceAttack = 0.0f;
    }

    private void PerformRoll()
    {
        m_rolling = true;
        m_animator.SetTrigger("Roll");
        m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
    }

    private void PerformJump()
    {
        m_grounded = false; // Đặt trạng thái không còn tiếp đất
        m_isJumping = true; // Đặt trạng thái nhảy
        m_animator.SetBool("Grounded", m_grounded);

        // Giữ hướng nhảy theo hướng di chuyển
        m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce); // Áp dụng lực nhảy
        m_animator.SetTrigger("Jump"); // Kích hoạt hoạt hình nhảy
        Debug.Log("Nhân vật nhảy.");
    }

    private void HandleIdleState()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > Mathf.Epsilon)
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }
        else if (m_grounded && !m_isJumping) // Chỉ chuyển về idle khi không nhảy
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    // Sự kiện hoạt hình
    void AE_SlideDust()
    {
        Vector3 spawnPosition = m_facingDirection == 1 ? m_wallSensorR2.transform.position : m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            GameObject dust = Instantiate(m_slideDust, spawnPosition, transform.localRotation);
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}