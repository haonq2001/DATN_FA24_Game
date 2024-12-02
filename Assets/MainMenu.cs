using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{





    public GameObject playButton; // Nút Play
    public GameObject newGameButton; // Nút New Game

    void Start()
    {
        // Kiểm tra dữ liệu lưu trữ
        if (HasSavedGame())
        {
            playButton.SetActive(true);
            newGameButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
            newGameButton.SetActive(true);
        }
    }
    bool HasSavedGame()
    {
        // Kiểm tra nếu có dữ liệu liên quan đến vị trí hoặc màn chơi
        return PlayerPrefs.HasKey("CurrentScene") && PlayerPrefs.HasKey("PlayerPosX");
    }















    public void PlayGame () {


        // Kiểm tra nếu có dữ liệu đã lưu
        int sceneIndex = PlayerPrefs.GetInt("CurrentScene");
        SceneManager.LoadScene(sceneIndex); // Chuyển đến màn chơi đã lưu

    }
    public void NewGame()
    {


        PlayerPrefs.DeleteAll(); // Xóa dữ liệu cũ
        SceneManager.LoadScene(1); // Chuyển đến màn game 1 (màn chơi đầu tiên)

    }

    public void QuitGame(){
        Application.Quit();
   }
}
