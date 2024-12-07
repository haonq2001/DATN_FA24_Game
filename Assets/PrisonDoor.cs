using UnityEngine;
using System.Collections;

public class PrisonDoor : MonoBehaviour
{
    public GameObject thankyou; // Đối tượng hiển thị "Thank You"
    private Animator animator;
    private BoxCollider2D boxCollider; // Collider cho cửa (hoặc BoxCollider nếu là 3D)
    public GameObject contin; // Con tin cần di chuyển
    public Transform destination; // Điểm đến (đầu map)

    private SpriteRenderer continSprite; // SpriteRenderer của con tin

    private void Start()
    {
        // Lấy Animator và Box Collider từ GameObject
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider != null)
        {
            boxCollider.enabled = false; // Ban đầu tắt collider
        }

        if (thankyou != null)
        {
            thankyou.SetActive(false); // Đảm bảo "thankyou" không hiển thị ban đầu
        }

        // Lấy SpriteRenderer của con tin
        if (contin != null)
        {
            continSprite = contin.GetComponent<SpriteRenderer>();
            if (continSprite != null)
            {
                // Đặt con tin mờ ban đầu
                Color color = continSprite.color;
                color.a = 0.3f; // Alpha (độ trong suốt)
                continSprite.color = color;
            }
        }
    }

    public void OpenDoor()
    {
        if (animator != null)
        {
            Debug.Log("Cửa nhà giam đã mở!");
            animator.SetTrigger("open"); // Kích hoạt animation
        }
    }

    // Hàm này sẽ được gọi khi animation kết thúc (thông qua Animation Event)
    public void EnableCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = true; // Bật collider sau khi cửa mở
            Debug.Log("Collider của cửa đã được bật!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ShowThankYou());
        }
    }

    IEnumerator ShowThankYou()
    {
        if (thankyou != null)
        {
            thankyou.SetActive(true); // Hiển thị "Thank You"
            yield return new WaitForSeconds(5f); // Đợi 2 giây
            thankyou.SetActive(false); // Tắt "Thank You"

            // Di chuyển con tin đến điểm đầu map
            if (contin != null && destination != null)
            {
                StartCoroutine(MoveContinToDestination());
            }
        }
    }

    IEnumerator MoveContinToDestination()
    {
        float speed = 1000f; // Tốc độ di chuyển
        while (Vector2.Distance(contin.transform.position, destination.position) > 0.1f)
        {
            contin.transform.position = Vector2.MoveTowards(
                contin.transform.position,
                destination.position,
                speed * Time.deltaTime
            );
            yield return null;
        }
        Debug.Log("Con tin đã đến đầu map!");

        // Làm con tin đậm lên
        StartCoroutine(FadeInContin());
    }

    IEnumerator FadeInContin()
    {
        if (continSprite != null)
        {
            Color color = continSprite.color;
            while (color.a < 1f) // Tăng dần alpha đến 1
            {
                color.a += 0.05f; // Điều chỉnh tốc độ tăng alpha
                continSprite.color = color;
                yield return new WaitForSeconds(0.05f); // Đợi một khoảng thời gian nhỏ
            }
            Debug.Log("Con tin đã đậm lên!");
        }
    }
}
