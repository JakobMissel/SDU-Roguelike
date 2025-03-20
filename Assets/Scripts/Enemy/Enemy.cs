using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public GameObject currentTarget;
    public float attackRange = 2.5f;
    public float dashRange = 6f;
    public int damage = 5;

    void Awake()
    {
        
    }
}
