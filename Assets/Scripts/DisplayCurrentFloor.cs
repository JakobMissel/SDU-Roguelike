using TMPro;
using UnityEngine;

public class DisplayCurrentFloor : MonoBehaviour
{
    TextMeshProUGUI floorText;

    void Awake()
    {
        floorText = GetComponent<TextMeshProUGUI>();
        floorText.text = $"Floor: {GameEvents.FloorLevel.ToString()}";
    }
}
