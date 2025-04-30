using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathScreen : MonoBehaviour
{
    GameObject deathScreen;
    [SerializeField] int sceneToLoad;

    void Awake()
    {
        deathScreen = transform.GetChild(0).gameObject;
        if (deathScreen == null)
        {
            Debug.LogError("DeathScreen GameObject not found in the scene.");
            return;
        }
        deathScreen.SetActive(false);
    }
    void OnEnable()
    {
        GameEvents.OnPlayerDeath += ShowDeathScreen;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerDeath -= ShowDeathScreen;
    }

    void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
    }

    public void RestartGame()
    {
        deathScreen.SetActive(false);
        SceneManager.LoadScene(sceneToLoad);
    }
}
