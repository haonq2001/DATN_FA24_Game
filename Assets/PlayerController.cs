﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float jumpForce = 2f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public GameObject bullet;
    public Transform bulletPos;
    public static bool facingRight = true;
    private float castCooldown = 2f;
    private float lastCastTime = 0f;
    public GameObject swordCollider;
    public GameObject swordCollider1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        swordCollider.SetActive(false);
        swordCollider1.SetActive(false);
    }

    void Update()
    {
        Move();
        Jump();
        Cast();
        Attack();
        Attack1();

        // Kiểm tra nhấn phím Space để bắn đạn
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    PlayerShoot();
        //}
    }

    public void PlayerShoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        Bullet bulletScript = newBullet.GetComponent<Bullet>();

        Debug.Log("Player Facing Right: " + facingRight);

        if (!facingRight)
        {
            bulletScript.SetDirection(-1);
        }
        else
        {
            bulletScript.SetDirection(1);
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("attack");
        }
    }

    void Attack1()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("strike");
        }
    }

    public void ShowSword()
    {
        swordCollider.SetActive(true);
    }

    public void HideSword()
    {
        swordCollider.SetActive(false);
    }
    public void ShowSword1()
    {
        swordCollider1.SetActive(true);
    }

    public void HideSword1()
    {
        swordCollider1.SetActive(false);
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
            facingRight = false;
        }
        else if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
            facingRight = true;
        }

        animator.SetBool("isWalking", moveInput != 0);
        animator.SetBool("IsGround", isGrounded);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetTrigger("Jump");
        }
    }

    void Cast()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time - lastCastTime >= castCooldown)
        {
            animator.SetTrigger("cast");
            lastCastTime = Time.time;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsGround", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsGround", false);
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("bay"))
        {
            animator.SetTrigger("die");
            StartCoroutine(WaitForDeathAnimation());
        }
        if (collision.gameObject.CompareTag("ngonlua"))
        {
            Destroy(collision.gameObject);
        }
    }

    IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0;
    }




    // public void TakeDamage(int damage)
    // {
    // 	health -= damage;

    // 	StartCoroutine(DamageAnimation());

    // 	if (health <= 0)
    // 	{
    // 		// Die();
    // 	}
    // }

    // void Die()
    // {
    //     Time.timeScale = 0;
    // }
    // IEnumerator DamageAnimation()
    // {
    // 	SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

    // 	for (int i = 0; i < 3; i++)
    // 	{
    // 		foreach (SpriteRenderer sr in srs)
    // 		{
    // 			Color c = sr.color;
    // 			c.a = 0;
    // 			sr.color = c;
    // 		}

    // 		yield return new WaitForSeconds(.1f);

    // 		foreach (SpriteRenderer sr in srs)
    // 		{
    // 			Color c = sr.color;
    // 			c.a = 1;
    // 			sr.color = c;
    // 		}

    // 		yield return new WaitForSeconds(.1f);
    // 	}
    // }
}
