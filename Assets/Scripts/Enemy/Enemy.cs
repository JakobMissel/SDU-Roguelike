using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public GameObject currentTarget;
    public float attackRange = 2.5f;
    public int damage = 5;

    public void Attack(GameObject target)
    {
        FaceTarget(target);
        target.GetComponentInParent<Health>().TakeDamage(damage);
    }

    void FaceTarget(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction, target.transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
    }
}
