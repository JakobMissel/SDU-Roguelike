using UnityEngine;

public class ExitMenuManager : MonoBehaviour
{
    [SerializeField] GameObject exitMenuUI;

    void Start()
    {
        OnResumeGame();
    }
    void OnEnable()
    {
        GameEvents.OnExitMenu += ShowExitMenu;
    }

    void OnDisable()
    {
        GameEvents.OnExitMenu -= ShowExitMenu;
    }

    void ShowExitMenu()
    {
        if (exitMenuUI != null)
        {
            if(!exitMenuUI.activeSelf)
            {
                exitMenuUI.SetActive(true);
                Time.timeScale = 0f; 
            }
            else
            {
                OnResumeGame();
            }
        }
        else
        {
            Debug.LogWarning("Exit Menu UI is not assigned in the ExitMenuManager.");
        }
    }

    public void OnExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void OnResumeGame()
    {
        if (exitMenuUI != null)
        {
            exitMenuUI.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogWarning("Exit Menu UI is not assigned in the ExitMenuManager.");
        }
    }
}
