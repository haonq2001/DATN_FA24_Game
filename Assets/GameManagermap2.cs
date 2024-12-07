using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManagermap2 : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject scorengonlua;
    // Start is called before the first frame update
    public TextMeshProUGUI scoreText;
    public playermap2 player;

    private int score = 0;
    void Start()
    {
        {
            // Tải điểm từ PlayerPrefs nếu tồn tại
            if (PlayerPrefs.HasKey("PlayerScore"))
            {
                score = PlayerPrefs.GetInt("PlayerScore");
            }
            else
            {
                score = 0; // Nếu không có, bắt đầu từ 0
            }

            SetScoreText();
            // Tải trạng thái nếu có
            if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY") && PlayerPrefs.HasKey("PlayerPosZ"))
            {
                float x = PlayerPrefs.GetFloat("PlayerPosX");
                float y = PlayerPrefs.GetFloat("PlayerPosY");
                float z = PlayerPrefs.GetFloat("PlayerPosZ");

                Vector3 savedPosition = new Vector3(x, y, z);
                player.transform.position = savedPosition; // Đặt lại vị trí nhân vật
            }
        }
    }

        public void AddScore()
        {
            score++;
            PlayerPrefs.SetInt("PlayerScore", score);
            PlayerPrefs.Save();
            SetScoreText();
        }

        public void BlockScore()
        {
            score--;
            PlayerPrefs.SetInt("PlayerScore", score);
            PlayerPrefs.Save();
            SetScoreText();
        }

        public void SetScoreText()
        {
            scoreText.text = "" + score.ToString("n0");
        }

        public void GameOver()
        {
            Time.timeScale = 0;
            gameOverUI.SetActive(true);
        }

        public void ReStart()
        {
        PlayerPrefs.DeleteAll(); // Xóa toàn bộ dữ liệu đã lưu
        PlayerPrefs.Save();      // Đảm bảo lưu thay đổi
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Tải lại màn hiện tại
    }

        public void MainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void Quit()
        {
            Application.Quit();
        }

        void OnApplicationQuit()
        {
            PlayerPrefs.SetInt("PlayerScore", score);
            PlayerPrefs.Save();
            SaveGameState();
        }
        void SaveGameState()
        {
            // Lưu màn chơi hiện tại
            PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);

            // Lưu vị trí nhân vật
            Vector3 playerPosition = player.transform.position; // Giả sử bạn có đối tượng `player`
            PlayerPrefs.SetFloat("PlayerPosX", playerPosition.x);
            PlayerPrefs.SetFloat("PlayerPosY", playerPosition.y);
            PlayerPrefs.SetFloat("PlayerPosZ", playerPosition.z);

            PlayerPrefs.Save(); // Lưu tất cả
        }

    }
