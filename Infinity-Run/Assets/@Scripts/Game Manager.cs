using System;
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

    public int hp = 3;

    [Header("References")]
    public GameObject IntroUI;
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
        }

        if (State == GameState.Playing && hp <= 0)
        {
            PlayerScript.KillPlayer();

            EnemySpawner.SetActive(false);
            FoodSpawner.SetActive(false);
            GoldenSpawner.SetActive(false);
            
            State = GameState.Dead;
        }
        
        // 게임 오버 이후에 씬 로드
        if (State == GameState.Dead && Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene("main");
        }
        
    }
}
