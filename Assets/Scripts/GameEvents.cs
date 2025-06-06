using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    public static event Action OnSouthButton;
    public static void SouthButton() => OnSouthButton?.Invoke();

    public static event Action OnPlayerDeath;
    public static void PlayerDeath() => OnPlayerDeath?.Invoke();

    public static event Action OnGameRestart;
    public static void GameRestart() => OnGameRestart?.Invoke();

    public static event Action OnEnemyDeath;
    public static void EnemyDeath() => OnEnemyDeath?.Invoke();

    public static event Action OnExitMenu;
    public static void ExitMenu() => OnExitMenu?.Invoke();

    public static int FloorLevel = 1;

    public static int EnemyKillCount = 0;

    public static bool GameIsOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }

    void OnEnable()
    {
        OnEnemyDeath += UpdateEnemyKillCount;
        OnPlayerDeath += GameOver;
        OnGameRestart += GameRestarted;
    }


    void OnDisable()
    {
        OnEnemyDeath -= UpdateEnemyKillCount;
        OnPlayerDeath -= GameOver;
        OnGameRestart -= GameRestarted;
    }
    
    private void GameOver()
    {
        GameIsOver = true;
    }

    private void GameRestarted()
    {
        GameIsOver = false;
        EnemyKillCount = 0;
        FloorLevel = 1;
        Debug.Log("Game Restarted");
    }

    void UpdateEnemyKillCount()
    {
        EnemyKillCount++;
        Debug.Log("Enemy Kill Count: " + EnemyKillCount);
    }
}
