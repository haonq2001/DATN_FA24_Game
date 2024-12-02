using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public GameManager gameManager;
    public audioManager audioManager;
    public GameObject batnhacnen;
    public GameObject tatnhacnen;
    public GameObject batamthanh;
    public GameObject tatamthanh;
    public GameObject panelexit;

    private void Start()
    {

        // Kiểm tra xem audioManager.Instance đã được khởi tạo chưa
        if (audioManager.Instance != null)
        {
            bool isMusicMuted = audioManager.Instance.musicSource.mute;
            tatnhacnen.SetActive(isMusicMuted);
            batnhacnen.SetActive(!isMusicMuted);

            // Đồng bộ trạng thái âm thanh hiệu ứng (SFX)
            bool isSFXMuted = audioManager.Instance.sfxSource.mute;
            tatamthanh.SetActive(isSFXMuted);
            batamthanh.SetActive(!isSFXMuted);
        }
        else
        {
            Debug.LogError("audioManager.Instance is null!");
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Home()
    {
        panelexit.SetActive(true);
        Time.timeScale = 0;
    }
    public void exitno()
    {
        panelexit.SetActive(false);
        Time.timeScale = 0;
    }

    public void Restart()
    {
        // Kiểm tra và tìm lại audioManager.Instance nếu null
        if (audioManager.Instance == null)
        {
            audioManager.Instance = FindObjectOfType<audioManager>();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }


    public void tatnhacn()
    {
        if (audioManager.Instance != null)
        {
            tatnhacnen.SetActive(true);
            batnhacnen.SetActive(false);
            audioManager.Instance.ToggleMusic();
        }
        else
        {
            Debug.LogError("audioManager.Instance is null!");
        }
    }

    public void batnhacn()
    {
        if (audioManager.Instance != null)
        {
            tatnhacnen.SetActive(false);
            batnhacnen.SetActive(true);
            audioManager.Instance.ToggleMusic();
        }
        else
        {
            Debug.LogError("audioManager.Instance is null!");
        }
    }

    public void batamthanhok()
    {
        if (audioManager.Instance != null)
        {
            tatamthanh.SetActive(false);
            batamthanh.SetActive(true);
            audioManager.Instance.ToggleSFX();
        }
        else
        {
            Debug.LogError("audioManager.Instance is null!");
        }
    }

    public void tatamthanhok()
    {
        if (audioManager.Instance != null)
        {
            tatamthanh.SetActive(true);
            batamthanh.SetActive(false);
            audioManager.Instance.ToggleSFX();
        }
        else
        {
            Debug.LogError("audioManager.Instance is null!");
        }
    }
}
