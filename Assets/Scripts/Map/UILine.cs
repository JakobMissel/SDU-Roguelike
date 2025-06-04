using UnityEngine;

public class UILine : MonoBehaviour {
    private RectTransform rectTransform;
    public RectTransform EndObject;
    public RectTransform StartObject;
    public RectTransform LineRect;
    public void Initialize() {
        LineRect = GetComponent<RectTransform>();

        // Convert world positions of start and end to local positions relative to the line's parent
        Vector3 worldStart = StartObject.position;
        Vector3 worldEnd = EndObject.position;

        Vector3 localStart = LineRect.parent.InverseTransformPoint(worldStart);
        Vector3 localEnd = LineRect.parent.InverseTransformPoint(worldEnd);

        Vector3 direction = localEnd - localStart;
        float distance = direction.magnitude;

        // Position the line at the start
        LineRect.anchoredPosition = localStart;

        // Set size and rotation
        LineRect.sizeDelta = new Vector2(distance, 2f); // line thickness = 2
        LineRect.pivot = new Vector2(0, 0.5f);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        LineRect.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
