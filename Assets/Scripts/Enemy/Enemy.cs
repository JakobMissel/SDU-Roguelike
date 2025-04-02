using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public GameObject currentTarget;
    public float attackRange = 2.5f;
    public int damage = 5;

    public void Attack(GameObject target)
    {
        target.GetComponentInParent<Health>().TakeDamage(damage);
    }
}
