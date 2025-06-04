using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    [SerializeField] float damageMultiplier = 1;
    [SerializeField] Health health;
    [SerializeField] DashAbility dashAbility;
    public void TakeDamage(float amount, bool isPercentage = false)
    {
        amount *= damageMultiplier;
        if(dashAbility.isDashing)
        {
            return; // If the entity is dashing, do not take damage.
        }
        health.TakeDamage(amount, isPercentage);
    }
}
