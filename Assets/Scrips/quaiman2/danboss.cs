using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class danboss : MonoBehaviour
{
    public float damage = 10f; // Lượng sát thương của viên đạn
    public float lifetime = 2f; // Thời gian tồn tại của viên đạn trước khi tự hủy

    private void Start()
    {
        // Tự hủy viên đạn sau một thời gian nhất định
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu viên đạn va chạm với đối tượng có tag là "Player"
        if (collision.CompareTag("Player"))
        {
            // Gây sát thương cho nhân vật (giả định nhân vật có component Health)
            // Health playerHealth = collision.GetComponent<Health>();
            // if (playerHealth != null)
            // {
            //     playerHealth.TakeDamage(damage);
            // }
            
            // Hủy viên đạn sau khi va chạm
            Destroy(gameObject);
        }
        // else if (collision.CompareTag("Obstacle")) // Nếu viên đạn va chạm với chướng ngại vật
        // {
        //     // Hủy viên đạn khi va chạm với chướng ngại vật
        //     Destroy(gameObject);
        // }
    }
}
