using UnityEngine;
using System;
public class CreateNewMap : MonoBehaviour
{
    public GameObject PrefabMap;
    public GameObject currentMap;
    public void DestroyMap()
    {
        if (currentMap != null)
        {
            Destroy(currentMap);
        }
    }
    void Start() {
        ToggleMap.OnToggleMapUI(false);
    }
    public void GenerateNewMap()
    {
        // ToggleMap.OnToggleMapUI(false);
        DestroyMap();
        var map = Instantiate(PrefabMap, transform);
        currentMap = map;
        ToggleMap.OnToggleMapUI(false);
    }
    public static event Action CreateMap;
    public static void OnCreateMap() => CreateMap?.Invoke();
    void OnEnable()
    {
        CreateMap += GenerateNewMap;
    }
    void OnDisable()
    {
        CreateMap -= GenerateNewMap;
    }
}
