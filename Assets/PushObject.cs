using UnityEngine;

public class PushObject : MonoBehaviour
{
    public float pushForce = 5f;  // Lực đẩy
    public float maxDistance = 5f;  // Khoảng cách di chuyển tối đa
    private Vector2 startPosition;  // Vị trí ban đầu của vật
    private bool isPushed = false;  // Kiểm tra nếu vật được đẩy
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;  // Lưu lại vị trí ban đầu của vật
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra va chạm với nhân vật
        if (collision.gameObject.CompareTag("Player"))
        {
            // Bắt đầu đẩy vật khi va chạm
            isPushed = true;
        }
    }

    private void Update()
    {
        if (isPushed)
        {
            // Tính khoảng cách vật đã di chuyển
            float distanceMoved = Vector2.Distance(startPosition, new Vector2(transform.position.x, transform.position.y));

            // Nếu vật chưa di chuyển đủ khoảng cách, tiếp tục đẩy
            if (distanceMoved < maxDistance)
            {
                // Đẩy vật về phía nhân vật
                Vector2 direction = (new Vector2(transform.position.x, transform.position.y) - startPosition).normalized;
                rb.velocity = direction * pushForce;  // Đẩy vật

            }
            else
            {
                // Dừng vật khi di chuyển đủ khoảng cách
                rb.velocity = Vector2.zero;  // Dừng vận tốc
                isPushed = false;  // Kết thúc quá trình đẩy
            }
        }
        else
        {
            // Nếu vật đã dừng lại, đảm bảo vận tốc được đặt lại về 0
            rb.velocity = Vector2.zero;
        }
    }
}
