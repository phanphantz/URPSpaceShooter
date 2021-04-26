using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayCore : MonoBehaviour
{
    public static GameplayCore Instance;
    public bool paused;
    public bool ended;
    public int score;
    public int highScore;

    public Color goldColor;

    public Text scoreText;
    public Text highScoreText;

    public GameObject pausedMenu;
    public GameObject restartMenu;

    private string filename = "savedata.es3";

    private void Awake() 
    {
        if (Instance && Instance != this)
            Destroy(gameObject);

        Instance = this;
        scoreText.text = "0";
        
        if (ES3.KeyExists("Highscore" , filename))
            highScore = ES3.Load<int>("Highscore" , filename);

        highScoreText.text = highScore.ToString();
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();

        if (score > highScore)
        {
            scoreText.color = goldColor;
            highScoreText.text = highScore.ToString();
            highScore = score;
            ES3.Save<int>("Highscore" , highScore, filename);
        }
        else
        {
            scoreText.color = Color.white;
        }
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !ended)
        {
            paused = !paused;
            Time.timeScale = paused? 0 : 1;
            pausedMenu.gameObject.SetActive(paused);
        }
    }

    public void InvokeResume()
    {
        if (ended)
            return;

        paused = false;
        Time.timeScale = 1;
        pausedMenu.gameObject.SetActive(false);
    }

    public void EndGame()
    {
        ended = true;
        restartMenu.gameObject.SetActive(true);
    }

    public void InvokeQuit()
    {
        Application.Quit();
    }

    public void InvokeRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
