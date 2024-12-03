using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private bool isPaused = false; // Biến để kiểm tra boss có bị dừng hay không



    public GameObject thanhmau1;
    public GameObject thanhmau2;
    public Slider hpboss1;
    public Slider hpboss2;
    public Image fillhp;
    public Image fillhp2;
    public float hp1 = 10;
    public float hp2 = 10;
    

    #region Public Variables
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;
    #endregion

    #region Private Variables
    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool cooling;
    private float intTimer;
    #endregion

    public GameObject tuongda1;
    public GameObject tuongda2;
    public GameObject boxtuongda;

    public GameObject hieuungtroi;
    

    void Awake()
    {
        anim = GetComponent<Animator>();
        intTimer = timer;
        SelectTarget();
        hpboss1.maxValue = hp1;

        hpboss1.value = hp1;
        hpboss1.interactable = false;
        hpboss2.maxValue = hp2;
        hpboss2.value = hp2;
        hpboss2.interactable = false;
    }

    void Update()
    {
        if (isPaused) return;
        if (!attackMode)
        {
            Move();
        }

        if (!InsideofLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            EnemyLogic();
        }
    }

    void EnemyLogic()
    {
        if (target == null) return; // Kiểm tra null

        distance = Vector2.Distance(transform.position, target.position);
        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (distance <= attackDistance && !cooling)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("Attack", false);
        }
    }

    void Move()
    {
        anim.SetBool("canWalk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack"))
        {
            if (target != null) // Kiểm tra null
            {
                Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                Flip(); // Gọi Flip mỗi khi di chuyển
            }
        }
    }

    void Attack()
    {
        timer = intTimer;
        attackMode = true;
        anim.SetBool("canWalk", false);
        anim.SetBool("Attack", true);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        if (leftLimit == null || rightLimit == null) return;

        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        target = (distanceToLeft > distanceToRight) ? leftLimit : rightLimit;

        if (target != null) Flip();
        else Debug.LogWarning("Target is null; cannot flip!");
    }

    public void Flip()
    {
        if (target == null) return; // Kiểm tra null trước khi quay

        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180;
        }
        else
        {
            rotation.y = 0;
        }
        transform.eulerAngles = rotation;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bulletpow"))
        {
            hieuungtroi.SetActive(true);
            StartCoroutine(PauseBossForSeconds(3f)); // Tạm dừng boss trong 3 giây
        }
            if (collision.CompareTag("Bullet") || collision.CompareTag("Sword"))
        {
            hpboss1.value -= 2;
         //   animator.SetTrigger("hulk");
            audioManager.Instance.PlaySFX("matmau");


            if (hpboss1.value <= 0)
            {
                Destroy(thanhmau1);
                hpboss2.value -= 2;

                if (hpboss2.value <= 0)
                {
                    StartCoroutine(WaitForDeathAnimation());

                }
            }
        }
    }
    IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);  // Destroy the boss
        Destroy(thanhmau2);
        boxtuongda.SetActive(true);
        if (tuongda1 != null)
        {
            StatueController statueController = tuongda1.GetComponent<StatueController>();
            if (statueController != null)
            {
                statueController.PushUp(); // Gọi hàm đẩy lên
            }

        }
        if (tuongda2 != null)
        {
            StatueController statueController = tuongda2.GetComponent<StatueController>();
            if (statueController != null)
            {
                statueController.PushUp(); // Gọi hàm đẩy lên
            }

        }
    }
    IEnumerator PauseBossForSeconds(float duration)
    {
        isPaused = true; // Dừng boss
        anim.SetBool("canWalk", false); // Tắt hoạt ảnh đi lại
        anim.SetBool("Attack", false); // Tắt hoạt ảnh tấn công
        yield return new WaitForSeconds(duration); // Đợi thời gian được chỉ định
        isPaused = false; // Tiếp tục boss
        hieuungtroi.SetActive(false); // Tắt hiệu ứng nếu cần
    }

}