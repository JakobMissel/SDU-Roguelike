using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField] Vector3 offset = new(0, 17, -11);
    [SerializeField] Vector3 initialRotation = new(55,0,0);
    [SerializeField] float minZoom = 30;
    [SerializeField] float maxZoom = 50;
    [SerializeField] float zoomSpeed = 2f;
    [SerializeField] [Range(0.001f, 1)] float followSpeed = 0.03f;

    CinemachineCamera cinemachineCamera;

    void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    void FixedUpdate()
    {
        if (PlayerDistanceManager.Instance == null)
        {
            Debug.LogWarning("PlayerDistanceManager instance is null. Camera movement will not be applied.");
            return;
        }
        Vector3 midpoint = PlayerDistanceManager.Instance.playersMidpointGO.transform.position;
        Vector3 desiredPosition = midpoint + offset;
        
        // Smoothly follow the desired position.
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed);
        transform.eulerAngles = initialRotation;
        float normalDistance = PlayerDistanceManager.Instance.normalDistance;
        
        // Adjust camera zoom based on the normal distance between players
        float newZoom = Mathf.Lerp(minZoom,maxZoom,normalDistance);
        cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(cinemachineCamera.Lens.FieldOfView, newZoom, zoomSpeed * Time.deltaTime);
    }

}
