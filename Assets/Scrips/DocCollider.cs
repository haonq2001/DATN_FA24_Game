using UnityEngine;

public class WaterDropCollider : MonoBehaviour
{
    private Collider waterCollider;

    private void Start()
    {
        waterCollider = GetComponent<Collider>();
    }

    // Hàm gọi từ Animation Event để thay đổi kích thước hoặc vị trí của Collider
    public void UpdateCollider(Vector3 newCenter, Vector3 newSize)
    {
        if (waterCollider is BoxCollider boxCollider)
        {
            boxCollider.center = newCenter;
            boxCollider.size = newSize;
        }
        // Nếu dùng SphereCollider hoặc CapsuleCollider, thay đổi tương ứng:
        else if (waterCollider is SphereCollider sphereCollider)
        {
            sphereCollider.center = newCenter;
            sphereCollider.radius = newSize.x; // Dùng x làm bán kính
        }
    }
}
