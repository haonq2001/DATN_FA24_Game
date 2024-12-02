using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

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

    void Awake()
    {
        anim = GetComponent<Animator>();
        intTimer = timer;
        SelectTarget();
    }

    void Update()
    {
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
}