﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 2f;
    public float jumpForce = 2f;
    private  Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer; // Biến để tham chiếu SpriteRenderer
    private Animator animator;
    public GameObject bullet;
    public Transform bulletPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Lấy tham chiếu SpriteRenderer
        animator = GetComponent<Animator>(); // Lấy tham chiếu Animator
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Cast();
    }
    public void PlayerShoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        if (moveInput < 0) // Di chuyển sang trái
        {
            spriteRenderer.flipX = true; // Quay sang trái
        }
        else if (moveInput > 0) // Di chuyển sang phải
        {
            spriteRenderer.flipX = false; // Quay sang phải
        }
        animator.SetBool("isWalking", moveInput != 0);
        animator.SetBool("isGrounded", isGrounded); // Cập nhật trạng thái mặt đất
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;  // Đặt isGrounded thành false ngay khi nhảy
            animator.SetTrigger("Jump"); // Gọi animation nhảy
        }
    }
    void Cast()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("cast"); // Gọi animation chưởng
         //   PlayerShoot();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra va chạm với mặt đất để xác định trạng thái isGrounded
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
}
