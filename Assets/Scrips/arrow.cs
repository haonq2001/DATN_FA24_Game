using UnityEngine;

public class arrow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu va chạm với người chơi hoặc vật cản
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Arrow hit the player!"); 
            Destroy(gameObject);        
        }
    }
}
