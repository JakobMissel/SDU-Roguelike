using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event Action OnSouthButton;
    public static void SouthButton() => OnSouthButton?.Invoke();

    public static event Action OnPlayerDeath;
    public static void PlayerDeath() => OnPlayerDeath?.Invoke();

    public static event Action OnEnemyDeath;
    public static void EnemyDeath() => OnEnemyDeath?.Invoke();

    public static int FloorLevel = 1;

    public static int EnemyKillCount = 0;

    public static bool GameIsOver = false;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void OnEnable()
    {
        OnEnemyDeath += UpdateEnemyKillCount;
        OnPlayerDeath += GameOver;
    }


    void OnDisable()
    {
        OnEnemyDeath -= UpdateEnemyKillCount;
        OnPlayerDeath -= GameOver;
    }
    
    private void GameOver()
    {
        GameIsOver = true;
    }

    void UpdateEnemyKillCount()
    {
        EnemyKillCount++;
    }
}
