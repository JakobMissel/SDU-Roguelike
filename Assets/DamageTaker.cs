using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    [SerializeField] float damageMultiplier = 1;
    public void TakeDamage(float amount, bool isPercentage = false)
    {
        amount *= damageMultiplier;
        GetComponentInParent<Health>().TakeDamage(amount, isPercentage);
    }
}
