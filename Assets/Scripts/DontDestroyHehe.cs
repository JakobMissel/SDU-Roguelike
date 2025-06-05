using UnityEngine;

public class DontDestroyHehe : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
