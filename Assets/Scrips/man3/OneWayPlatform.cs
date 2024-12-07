using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform  : MonoBehaviour
{
     private PlatformEffector2D effector;
    public float waitTime = 0.2f;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        // Nhấn phím "S" để nhảy xuyên qua nền
        if (Input.GetKeyDown(KeyCode.S))
        {
            effector.rotationalOffset = 180f; // Cho phép xuyên từ dưới lên
            StartCoroutine(ResetEffector());
        }
    }

    IEnumerator ResetEffector()
    {
        yield return new WaitForSeconds(waitTime);
        effector.rotationalOffset = 0f; // Trả về trạng thái ban đầu
    }
}
