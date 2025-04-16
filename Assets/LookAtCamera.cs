using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera camera;
    void Awake()
    {
        camera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(camera.transform);
    }
}
