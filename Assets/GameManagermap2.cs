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
    private int score = 0;
    void Start()
    {
        SetScoreText();
        // Cursor.visible  = false;
        //  Cursor.lockState = CursorLockMode.Locked;
    }
    public void AddScore()
    {
        score++;
    }
    public void BlockScore()
    {
        if (score > 0)
        {
            score--;
        }
        else
        {
            score = 0;
        }
        
    }
    public void SetScoreText()
    {
        scoreText.text = "" + score.ToString("n0");
    }
    // Update is called once per frame
    void Update()
    {
        //if(gameOverUI.activeInHierarchy){
        //    Cursor.visible = true;
        //    Cursor.lockState = CursorLockMode.None;
        //}
        //else{
        //    Cursor.visible = false;
        //    Cursor.lockState = CursorLockMode.Locked;
        //}
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverUI.SetActive(true);

    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
