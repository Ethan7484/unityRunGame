using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    Intro, 
    Playing,
    Dead
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState State = GameState.Intro;

    public float playStartTime;

    public int hp = 3;

    [Header("References")]
    public GameObject introUI;
    public GameObject deadUI;
    [Space]
    public GameObject buildingSpawner;
    public GameObject enemySpawner;
    public GameObject foodSpawner;
    public GameObject goldenSpawner;
    [Space] 
    public Player playerScript;
    public TMP_Text scoreText;

    [Header("Buttons")]
    public GameObject startButton;
    public GameObject restartButton;
    public GameObject jumpButton;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }        
    }

    private void Start()
    {
        introUI.SetActive(true);

        buildingSpawner.SetActive(false);
        enemySpawner.SetActive(false);
        foodSpawner.SetActive(false);
        goldenSpawner.SetActive(false);
        restartButton.SetActive(false);

        startButton.SetActive(true);
        jumpButton.SetActive(false);
    }

    private void Update()
    {

        if (State == GameState.Intro)
        {
            scoreText.text = "HIGH SCORE: " + GetHighScore();
        }

        if (State == GameState.Playing)
        {
            scoreText.text = "SCORE: " + Mathf.FloorToInt(CalculateScore());
        }
        else if (State == GameState.Dead)
        {
            scoreText.text = "HIGH SCORE: " + GetHighScore();
        }        


        if (State == GameState.Playing && hp <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        playerScript.KillPlayer();

        buildingSpawner.SetActive(false);
        enemySpawner.SetActive(false);
        foodSpawner.SetActive(false);
        goldenSpawner.SetActive(false);

        jumpButton.SetActive(false);

        SaveHighScore();

        deadUI.SetActive(true);

        restartButton.SetActive(true);
        startButton.SetActive(false);

        State = GameState.Dead;
    }

    public void RestartButton()
    {
        restartButton.SetActive(false);
        startButton.SetActive(true);
        SceneManager.LoadScene("main");
    }

    public void StartButton()
    {
        State = GameState.Playing;
        introUI.SetActive(false);

        startButton.SetActive(false);
        jumpButton.SetActive(true);

        buildingSpawner.SetActive(true);
        enemySpawner.SetActive(true);
        foodSpawner.SetActive(true);
        goldenSpawner.SetActive(true);

        playStartTime = Time.time;
    }

    float CalculateScore()
    {
        return Time.time - playStartTime;
    }

    void SaveHighScore()
    {
        int score = Mathf.FloorToInt(CalculateScore());
        int currentHighScore = PlayerPrefs.GetInt("HighScore");

        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }

    public float CalculateGameSpeed()
    {
        if (State != GameState.Playing)
        {
            return 5f;
        }
        
        float speed  = 8f + (0.5f * MathF.Floor(CalculateScore() / 10f));    // 1초에 0.5f 씩 증가

        return Mathf.Min(speed, 30f);

    }
    
    
}
