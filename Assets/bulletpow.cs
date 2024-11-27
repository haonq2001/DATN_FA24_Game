using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletpow : MonoBehaviour
{ // Rigidbody2D rb;
    // Start is called before the first frame update
    public float speed = 8f; // Speed of the bullet
    private int direction; // Direction of the bullet

    void Start()
    {
        Destroy(gameObject, 5f); // Destroy bullet after 3 seconds
    }

    public void SetDirection(int dir)
    {
        direction = dir; // Set direction: 1 for right, -1 for left
        Debug.Log("Bullet Direction Set To: " + direction);
        transform.localScale = new Vector3(dir, 1, 1); // Optionally flip the bullet's scale for visual feedback
    }

    void Update()
    {
        // Move the bullet based on the direction
        transform.position += new Vector3(direction * speed * Time.deltaTime, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

    }
}
