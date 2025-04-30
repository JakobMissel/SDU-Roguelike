using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    public static void PlayerDeath() => OnPlayerDeath?.Invoke();

    public static event Action OnEnemyDeath;
    public static void EnemyDeath() => OnEnemyDeath?.Invoke();

    public static int FloorLevel = 1;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
