using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thanhmau : MonoBehaviour
{
    public Transform boss; // Tham chiếu đến GameObject của Boss
    public Vector3 offset; // Khoảng cách giữa thanh máu và Boss

    void Update()
    {
        if (boss != null)
        {
            // Cập nhật vị trí của thanh máu theo Boss
            transform.position = boss.position + offset;
        }
    }
}