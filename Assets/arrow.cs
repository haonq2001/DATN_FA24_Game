using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu mũi tên va chạm với một vật cản
        if (collision.CompareTag("Player")) // Nếu gặp người chơi
        {
            Debug.Log("Arrow hit the player!");
            // Bạn có thể thêm xử lý logic khác tại đây (ví dụ: giảm máu người chơi)
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Arrow hit an obstacle!");
        }

        // Hủy mũi tên ngay khi va chạm
        
    }
}
