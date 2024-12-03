using UnityEngine;

public class StatueController : MonoBehaviour
{
    public float pushForce = 1000f; // Lực đẩy, có thể chỉnh trong Inspector

    public void PushUp()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Chuyển từ Kinematic sang Dynamic
            rb.AddForce(Vector2.up * pushForce);  // Đẩy lên trên
        }
        else
        {
            Debug.LogWarning("Statue doesn't have a Rigidbody2D component!");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("boxtuongda"))
        {
            Destroy(gameObject);
        }
    }
}
