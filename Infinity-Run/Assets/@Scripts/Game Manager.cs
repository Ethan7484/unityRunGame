using System;
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
    public GameObject IntroUI;
    public GameObject DeadUI;
    [Space]
    public GameObject EnemySpawner;
    public GameObject FoodSpawner;
    public GameObject GoldenSpawner;
    [Space] 
    public Player PlayerScript;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    private void Start()
    {
        IntroUI.SetActive(true);
        
        EnemySpawner.SetActive(false);
        FoodSpawner.SetActive(false);
        GoldenSpawner.SetActive(false);
    }

    private void Update()
    {
        if (State == GameState.Intro && Input.GetKeyDown(KeyCode.Tab ))
        {
            State = GameState.Playing;
            IntroUI.SetActive(false);
            
            EnemySpawner.SetActive(true);
            FoodSpawner.SetActive(true);
            GoldenSpawner.SetActive(true);
            
            playStartTime = Time.time;
        }

        if (State == GameState.Playing && hp <= 0)
        {
            PlayerScript.KillPlayer();

            EnemySpawner.SetActive(false);
            FoodSpawner.SetActive(false);
            GoldenSpawner.SetActive(false);
            
            DeadUI.SetActive(true);
            
            State = GameState.Dead;
        }
        
        // 게임 오버 이후에 씬 로드
        if (State == GameState.Dead && Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene("main");
        }
        
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
    
    
}
