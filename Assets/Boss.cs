using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
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
    public GameObject batnhacnen;
    public GameObject tatnhacnen;
    public GameObject batamthanh;
    public GameObject tatamthanh;
    public audioManager audioManager;
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
            BossHealth.value -= 2;
            animator.SetBool("hulk", true);
            audioManager.Instance.PlaySFX("matmau");
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
        if (torchPrefab != null && dropPoint != null)
        {
            Instantiate(torchPrefab, dropPoint.position, Quaternion.identity);  // Drop the torch at dropPoint
        }
    }
}