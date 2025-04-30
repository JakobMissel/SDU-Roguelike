using UnityEngine;

public class UILine : MonoBehaviour {
    private RectTransform rectTransform;
    public RectTransform EndObject;
    public RectTransform StartObject;
    public RectTransform LineRect;
    void Start() {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.position = StartObject.position;
        Vector3 dir = EndObject.position - StartObject.position;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, dir.magnitude);
        rectTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg+90f);
    }
}
