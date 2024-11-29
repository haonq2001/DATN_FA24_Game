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
        // Ki?m tra n?u m?i tên va ch?m v?i m?t v?t c?n
        if (collision.CompareTag("Player")) // N?u g?p ng??i ch?i
        {
            Debug.Log("Arrow hit the player!");
            // B?n có th? thêm x? lý logic khác t?i ?ây (ví d?: gi?m máu ng??i ch?i)
        }
        else
        {
            Debug.Log("Arrow hit an obstacle!");
        }

        // H?y m?i tên ngay khi va ch?m
        Destroy(gameObject);
    }
}
