using System;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField] [Tooltip("The camera's offset on the y-axis (the distance from the ground). \nDefault: 15")] float yOffset= 15f;
    [SerializeField] [Tooltip("Invert the movement on the y-axis")] bool invertY;
    [SerializeField] [Tooltip("The camera's offset on the z-axis (the distance from the players). \nDefault: -20")] float zOffset= -20f;
    [SerializeField] [Tooltip("Invert the movement on the y-axis")] bool invertZ;
    [SerializeField] [Tooltip("Zoom multiplier for the camera. \nDefault: 1")] float zoomMultiplier;
    void Update()
    {
        Vector3 trackTarget = PlayerDistanceManager.Instance.playersMidpointGO.transform.position;
        float newYOffset;
        float newZOffset;
        if (invertY)
        {
            newYOffset = yOffset - PlayerDistanceManager.Instance.currentDistance;
        }
        else
        {
            newYOffset = yOffset + PlayerDistanceManager.Instance.currentDistance * zoomMultiplier;
        }
        if (invertZ)
        {
            newZOffset = zOffset + PlayerDistanceManager.Instance.currentDistance * zoomMultiplier;
        }
        else
        {
            newZOffset = zOffset - PlayerDistanceManager.Instance.currentDistance * zoomMultiplier;
        }
        transform.position = new Vector3(trackTarget.x, trackTarget.y + newYOffset, trackTarget.z + newZOffset);
    }
}
