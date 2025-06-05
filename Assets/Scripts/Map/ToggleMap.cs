using UnityEngine;
public class ToggleMap : MonoBehaviour {
    public GameObject MapUI;
    public MapUI mapUIComponent;
    public GameObject MapUI2;
    public UnityEngine.UI.Image MapBackground;
    public static event System.Action<bool> ToggleMapUI;
    public static void OnToggleMapUI(bool value) => ToggleMapUI?.Invoke(value);

    void OnEnable()
    {
        ToggleMapUI += ToggleMapView;
    }
    void OnDisable()
    {
        ToggleMapUI -= ToggleMapView;
    }
    // void Start()
    // {
    //     ToggleMapView(false);
    // }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ToggleMapView(bool value)
    {
        mapUIComponent.ToggleMapMapInput(value);
        MapUI.SetActive(value);
        MapUI2.SetActive(value);
        MapBackground.enabled = value;

    }
}
