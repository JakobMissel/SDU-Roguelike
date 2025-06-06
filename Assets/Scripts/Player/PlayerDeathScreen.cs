using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathScreen : MonoBehaviour
{
    GameObject deathScreen;
    [SerializeField] int sceneToLoad;
    [SerializeField] TextMeshProUGUI killCountTMP;
    [SerializeField] [TextArea] string killCountText;
    bool gameOver = false;

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
        GameEvents.OnSouthButton += RestartGame;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerDeath -= ShowDeathScreen;
        GameEvents.OnSouthButton -= RestartGame;
    }

    void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        StartCoroutine(SetText("", killCountText));
        gameOver = true;
    }

    public void RestartGame()
    {
        if (!gameOver) return;
        deathScreen.SetActive(false);
        gameOver = false;
        CreateNewMap.OnCreateMap();
        GameEvents.GameRestart();
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator SetText(string text1, string text2)
    {
        killCountTMP.text = text1;
        yield return new WaitForSeconds(1f);
        foreach (char letter in text2.ToCharArray())
        {
            killCountTMP.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        
        for (int i = 1; i <= GameEvents.EnemyKillCount; i++)
        {
            killCountTMP.text = $"{text2}\n{(i)}";
            yield return new WaitForSeconds(0.05f);
        }
    }
}
