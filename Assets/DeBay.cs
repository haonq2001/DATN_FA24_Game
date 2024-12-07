using System.Collections;
using UnityEngine;

public class ArrowShooter : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab của mũi tên
    public Transform viTriBan;     // Điểm bắn (vị trí xuất phát của mũi tên)
    public float arrowLifetime = 5f; // Thời gian tồn tại của mũi tên nếu không gặp vật cản
    public float fireInterval = 5f; // Khoảng thời gian giữa các lần bắn
    public float arrowSpeed = 10f;  // Tốc độ bay của mũi tên

    private void Start()
    {
        // Bắt đầu chu kỳ bắn mũi tên
        StartCoroutine(FireArrows());
    }

    private IEnumerator FireArrows()
    {
        while (true)
        {
            // Tạo mũi tên
            GameObject arrow = Instantiate(arrowPrefab, viTriBan.position, viTriBan.rotation);

            // Gắn Rigidbody2D để mũi tên di chuyển
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = viTriBan.right * arrowSpeed; // Bay theo hướng của viTriBan
            }

            // Hủy mũi tên sau arrowLifetime giây nếu không gặp vật cản
            Destroy(arrow, arrowLifetime);

            // Đợi fireInterval giây trước khi bắn mũi tên tiếp theo
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
