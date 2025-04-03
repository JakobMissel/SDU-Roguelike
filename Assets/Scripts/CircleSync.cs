using System;
using UnityEngine;

public class CircleSync : MonoBehaviour
{
    public static int posID = Shader.PropertyToID("_Position");
    public static int sizeID = Shader.PropertyToID("_Size");
    
    [SerializeField] private Material wallMaterial;
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask layerMask;
    private void Update()
    {
        var dir = camera.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);
        
        if(Physics.Raycast(ray,3000,layerMask))
            wallMaterial.SetFloat(sizeID, 1);
        else
            wallMaterial.SetFloat(sizeID, 0);
        
        var view = camera.WorldToViewportPoint(transform.position);
        wallMaterial.SetVector(posID, view);
    }
}
