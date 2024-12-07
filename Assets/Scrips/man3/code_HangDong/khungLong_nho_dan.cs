using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class khungLong_nho_dan : MonoBehaviour
{
    //   public float speed = 1f;             // Tốc độ di chuyển của ngọn lửa
    // public float lifeTime = 3f;          // Thời gian sống trước khi tự hủy
    // public int damage = 10;              // Sát thương của ngọn lửa
    // // public GameObject explosionEffect;   // Hiệu ứng khi va chạm

    // private Vector2 direction;           // Hướng di chuyển của ngọn lửa

    // void Start()
    // {
    //     // Tự hủy ngọn lửa sau một khoảng thời gian
    //     Destroy(gameObject, lifeTime);
    // }

    // void Update()
    // {
    //     // Di chuyển ngọn lửa theo hướng đã định
    //     transform.Translate(direction * speed * Time.deltaTime);
    // }

    // public void SetDirection(Vector2 dir)
    // {
    //     direction = dir.normalized; // Đặt hướng di chuyển
    // }

    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     // Kiểm tra va chạm với nhân vật hoặc vật thể
    //     if (collision.CompareTag("Player"))
    //     {
    //         // Gây sát thương cho nhân vật
    //         // collision.GetComponent<PlayerHealth>()?.TakeDamage(damage);

    //         // Tạo hiệu ứng nổ (nếu có)
    //         // if (explosionEffect != null)
    //         // {
    //         //     Instantiate(explosionEffect, transform.position, Quaternion.identity);
    //         // }

    //         // Hủy ngọn lửa
    //         Destroy(gameObject);
    //     }
    //     else if (collision.CompareTag("Ground") )
    //     {
    //         // Va chạm với mặt đất hoặc chướng ngại vật
    //         // if (explosionEffect != null)
    //         // {
    //         //     Instantiate(explosionEffect, transform.position, Quaternion.identity);
    //         // }

    //         Destroy(gameObject);
    //     }
    // }
     public float damage = 10f; // Lượng sát thương của viên đạn
    public float lifetime = 2f; // Thời gian tồn tại của viên đạn trước khi tự hủy

    private void Start()
    {
        // Tự hủy viên đạn sau một thời gian nhất định
        Destroy(gameObject, lifetime);
    }

   private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        // Lấy component Health từ nhân vật
        // PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        // if (playerHealth != null)
        // {
        //     playerHealth.TakeDamage(damage); // Gây sát thương
        // }

        // Hủy viên đạn sau khi va chạm
        Destroy(gameObject);
    }
    else if (collision.CompareTag("Ground"))
    {
        // Hủy viên đạn khi va chạm với mặt đất
        Destroy(gameObject);
    }
}

}
