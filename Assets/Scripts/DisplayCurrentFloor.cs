using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DisplayCurrentFloor : MonoBehaviour
{
    TextMeshProUGUI floorText;

    void Awake()
    {
        floorText = GetComponent<TextMeshProUGUI>();
        floorText.text = $"Floor: {GameEvents.FloorLevel.ToString()}";
    }
        void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        floorText.text = $"Floor: {GameEvents.FloorLevel.ToString()}";
    }
}
