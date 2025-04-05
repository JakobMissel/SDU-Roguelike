using System;
using UnityEngine;

public class CircleSync : MonoBehaviour
{
    public static int posID1 = Shader.PropertyToID("_PositionOne");
    public static int posID2 = Shader.PropertyToID("_PositionTwo");
    
    
    public static int sizeID = Shader.PropertyToID("_Size");
    
    [SerializeField] private Material material;
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask layerMask;
    private void Update()
    {
        var dir = camera.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);
        
        if(Physics.Raycast(ray,3000,layerMask))
            material.SetFloat(sizeID, 1);
        else
            material.SetFloat(sizeID, 0);
        
        var view = camera.WorldToViewportPoint(transform.position);
        material.SetVector(posID1, view);
    }
}
