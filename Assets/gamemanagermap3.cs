using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class gamemanagermap3 : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject scorengonlua;
    public TextMeshProUGUI scoreText;
    public playermap3 player;

    private int score = 0;

    public List<GameObject> enemies; // Danh sách chứa quái

    void Start()
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

        // Tải vị trí nhân vật
        if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY") && PlayerPrefs.HasKey("PlayerPosZ"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");

            Vector3 savedPosition = new Vector3(x, y, z);
            player.transform.position = savedPosition; // Đặt lại vị trí nhân vật
        }

        // Khôi phục trạng thái quái
        InitializeEnemies();

        // Nạp lại trạng thái máu và mana
        LoadPlayerState();
    }

    public void AddScore()
    {
        score++;
        PlayerPrefs.SetInt("PlayerScore", score);
        SetScoreText();
    }

    public void BlockScore()
    {
        score--;
        PlayerPrefs.SetInt("PlayerScore", score);
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
        SaveGameState();  // Lưu tất cả các dữ liệu game
        SavePlayerState(); // Lưu trạng thái nhân vật
    }

    void SavePlayerState()
    {
        PlayerPrefs.SetInt("PlayerHealth", player.health);  // Lưu máu
        PlayerPrefs.SetInt("PlayerMana", player.mana);      // Lưu mana
    }

    void LoadPlayerState()
    {
        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            player.health = PlayerPrefs.GetInt("PlayerHealth");
        }

        if (PlayerPrefs.HasKey("PlayerMana"))
        {
            player.mana = PlayerPrefs.GetInt("PlayerMana");
        }
    }

    void SaveGameState()
    {
        // Lưu màn chơi hiện tại
        PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);

        // Lưu vị trí nhân vật
        Vector3 playerPosition = player.transform.position;
        PlayerPrefs.SetFloat("PlayerPosX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPosZ", playerPosition.z);

        PlayerPrefs.Save(); // Lưu tất cả dữ liệu
    }

    // Lưu trạng thái quái
    public void SaveEnemyState(int enemyIndex, bool isDead)
    {
        string key = "Enemy" + enemyIndex + "Dead";
        PlayerPrefs.SetInt(key, isDead ? 1 : 0);  // Lưu trạng thái quái
    }

    void InitializeEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            string key = "Enemy" + i + "Dead";
            if (PlayerPrefs.GetInt(key, 0) == 1)
            {
                enemies[i].SetActive(false);  // Ẩn quái nếu đã chết
            }
            else
            {
                enemies[i].SetActive(true);  // Hiện quái nếu chưa chết
            }
        }
    }
}
