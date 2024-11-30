using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candauvancolider : MonoBehaviour
{
    public float moveSpeed = 5f;     // Tốc độ di chuyển của bậc
    public float moveDistance = 5f;  // Khoảng cách di chuyển của bậc

    private Vector3 initialPosition;
    private bool isMovingRight = true;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Di chuyển bậc sang trái hoặc phải theo chiều x
        if (isMovingRight)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        // Đảo hướng di chuyển khi bậc đạt đủ khoảng cách di chuyển
        if (transform.position.x >= initialPosition.x + moveDistance)
        {
            isMovingRight = false;
            Flip(); // Lật đối tượng
        }
        else if (transform.position.x <= initialPosition.x)
        {
            isMovingRight = true;
            Flip(); // Lật đối tượng
        }
    }

    // Hàm để lật đối tượng
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Đảo chiều trục X
        transform.localScale = scale;
    }
}
