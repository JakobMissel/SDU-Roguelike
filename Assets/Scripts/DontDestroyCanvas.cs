using UnityEngine;

public class DontDestroyCanvas : MonoBehaviour {
    void Awake()
    {
        if (FindObjectsByType<Canvas>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        
    }
}
