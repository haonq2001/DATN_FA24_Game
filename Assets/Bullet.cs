using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    float speed = 5f;
    Vector2 moveDirection; // Vector lưu trữ hướng di chuyển

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3f);

        // Lấy hướng quay của nhân vật từ PlayerController
        bool isFacingRight = PlayerController.facingRight;

        // Thiết lập hướng di chuyển của viên đạn dựa trên hướng quay của nhân vật
        if (isFacingRight)
        {
            moveDirection = Vector2.right;
            rb.velocity = moveDirection * speed;
            Debug.Log("Di chuyển sang phải");
        }
        else
        {
            moveDirection = Vector2.left;
            rb.velocity = moveDirection * speed;
            Debug.Log("Di chuyển sang trái");
        }
    }

    // Các phương thức Update và huongdichuyen() khác
}