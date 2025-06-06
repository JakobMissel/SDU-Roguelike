using UnityEngine;

public class ExitMenuManager : MonoBehaviour
{
    public static ExitMenuManager Instance;

    [SerializeField] GameObject exitMenuUI;
    public bool exitMenuActive = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

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
                exitMenuActive = true;
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
            exitMenuActive = false;
        }
        else
        {
            Debug.LogWarning("Exit Menu UI is not assigned in the ExitMenuManager.");
        }
    }
}
