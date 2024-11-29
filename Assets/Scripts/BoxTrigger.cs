using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    public GameObject statue1; // Tượng 1
    public GameObject statue2; // Tượng 2

    /// <summary>
    /// Gọi khi một đối tượng khác đi vào Trigger Collider gắn với đối tượng này (2D Physics).
    /// </summary>
    /// <param name="other">Đối tượng Collider2D tham gia va chạm.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu đối tượng va chạm có thẻ "Player"
        if (other.CompareTag("Player"))
        {
            // Kích hoạt trọng lực cho tượng 1 nếu có
            if (statue1 != null && statue1.GetComponent<Rigidbody2D>() != null)
            {
                statue1.GetComponent<Rigidbody2D>().gravityScale = 1f; // Kích hoạt trọng lực
            }
            else
            {
                Debug.LogWarning("statue1 không tồn tại hoặc không có Rigidbody2D!");
            }

            // Kích hoạt trọng lực cho tượng 2 nếu có
            if (statue2 != null && statue2.GetComponent<Rigidbody2D>() != null)
            {
                statue2.GetComponent<Rigidbody2D>().gravityScale = 1f; // Kích hoạt trọng lực
            }
            else
            {
                Debug.LogWarning("statue2 không tồn tại hoặc không có Rigidbody2D!");
            }
        }
    }
}
