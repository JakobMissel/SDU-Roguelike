using UnityEngine;

public class DontDestroy : MonoBehaviour {
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
