using UnityEngine;

public class DontDestroyHehe : MonoBehaviour
{
    public static DontDestroyHehe Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
    }
}
