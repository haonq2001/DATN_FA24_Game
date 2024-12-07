using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Check_dichuyenvaohang : MonoBehaviour
{
     public Transform newPosition;          // Vị trí cửa hang bên dưới
    public CinemachineVirtualCamera virtualCamera; // Camera sử dụng
    public FadeEffect fadeEffect;          // Hiệu ứng fade
    public float moveDuration = 1f;        // Thời gian di chuyển nhân vật

    private bool isTransitioning = false;  // Biến kiểm tra trạng thái di chuyển

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            StartCoroutine(TransitionPlayer(other.transform));
        }
    }

    private IEnumerator TransitionPlayer(Transform player)
    {
        isTransitioning = true;

        // Bắt đầu hiệu ứng fade out
        yield return StartCoroutine(fadeEffect.FadeOut());

        // Lưu vị trí ban đầu của nhân vật
        Vector3 startPos = player.position;
        float elapsedTime = 0f;

        // Di chuyển nhân vật mượt mà đến vị trí mới
        while (elapsedTime < moveDuration)
        {
            player.position = Vector3.Lerp(startPos, newPosition.position, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo nhân vật ở đúng vị trí cuối
        player.position = newPosition.position;

        // Nếu sử dụng Cinemachine, cập nhật vị trí camera
        if (virtualCamera != null)
        {
            virtualCamera.transform.position = new Vector3(
                newPosition.position.x,
                newPosition.position.y,
                virtualCamera.transform.position.z
            );
        }

        // Bắt đầu hiệu ứng fade in
        yield return StartCoroutine(fadeEffect.FadeIn());

        isTransitioning = false;
    }
}
