using UnityEngine;

public class TimeControl : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] float timeScale = 1;
    void Update()
    {
        Time.timeScale = timeScale;
    }
}
