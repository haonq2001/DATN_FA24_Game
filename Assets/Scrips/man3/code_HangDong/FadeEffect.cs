using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
     public Image fadeImage; // Image được sử dụng để fade
    public float fadeDuration = 1f; // Thời gian thực hiện fade

    private void Start()
    {
        fadeImage.color = new Color(0, 0, 0, 0); // Bắt đầu với trong suốt
    }

    // Hàm fade out (ẩn nhân vật)
    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, elapsedTime / fadeDuration)); // Tăng alpha
            yield return null;
        }
    }

    // Hàm fade in (hiện nhân vật)
    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, elapsedTime / fadeDuration)); // Giảm alpha
            yield return null;
        }
    }
}

