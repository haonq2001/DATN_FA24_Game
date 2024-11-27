using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiaset : MonoBehaviour
{
    public int damage = 10; // Lượng máu bị trừ khi tia sét va chạm
    public float destroyDelay = 1.5f; // Thời gian giữ tia sét trước khi xóa

    // Update không cần thiết để tia sét đứng yên tại chỗ

    // Hàm xử lý khi tia sét va chạm với mục tiêu
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // // Trừ máu player
            // PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            // if (playerHealth != null)
            // {
            //     playerHealth.TakeDamage(damage); // Gọi hàm trừ máu
            // }
            Debug.Log("Tia set cham player");
            StartCoroutine(DestroyAfterDelay()); // Gọi Coroutine trì hoãn xóa tia sét
        }
        else if (other.CompareTag("Ground"))
        {
            Debug.Log("Tia set cham ground");
            StartCoroutine(DestroyAfterDelay()); // Gọi Coroutine trì hoãn xóa tia sét
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        // Đợi một khoảng thời gian trước khi xóa tia sét
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject); // Xóa tia sét
    }
}
