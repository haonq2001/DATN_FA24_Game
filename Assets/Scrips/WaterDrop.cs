using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    private Vector3 targetPosition;
    private float speed;
    private float destroyDelay = 1.5f; // Thời gian chờ trước khi Destroy

    public void Initialize(Vector3 target, float dropSpeed)
    {
        targetPosition = target;
        speed = dropSpeed;
    }

    private void Update()
    {
        // Di chuyển giọt nước từ vị trí hiện tại tới target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Kiểm tra nếu giọt nước đã đến điểm B
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Kích hoạt quá trình hủy sau một khoảng thời gian
            Invoke("DestroyWaterDrop", destroyDelay);
            enabled = false; // Ngừng di chuyển sau khi đến đích
        }
    }

    private void DestroyWaterDrop()
    {
        Destroy(gameObject); // Xóa giọt nước sau thời gian chờ
    }
}
