using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muiTen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ki?m tra n?u m?i t�n va ch?m v?i m?t v?t c?n
        if (collision.CompareTag("Player")) // N?u g?p ng??i ch?i
        {
            Debug.Log("Arrow hit the player!");
            // B?n c� th? th�m x? l� logic kh�c t?i ?�y (v� d?: gi?m m�u ng??i ch?i)
        }
        else
        {
            Debug.Log("Arrow hit an obstacle!");
        }

        // H?y m?i t�n ngay khi va ch?m
        Destroy(gameObject);
    }
}
