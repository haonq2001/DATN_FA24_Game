using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject scorengonlua;
    // Start is called before the first frame update
    public TextMeshProUGUI scoreText;
    private int score = 0;
    void Start()
    {
        SetScoreText();
    }
    public void AddScore()
    {
        score++;
    }
    public void BlockScore()
    {
        score--;
    }
    public void SetScoreText()
    {
        scoreText.text = "" + score.ToString("n0");
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void GameOver(){
        gameOverUI.SetActive(true);
    }

    public void ReStart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu(){
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit(){
        Application.Quit();
    }
}
