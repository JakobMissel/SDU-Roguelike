using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    [SerializeField] float damageMultiplier = 1;
    [SerializeField] Health health;
    [SerializeField] DashAbility dashAbility;
    [SerializeField] GameObject damageNumberPrefab;
    public void TakeDamage(float amount, bool isPercentage = false)
    {
        amount *= damageMultiplier;
        if(dashAbility.isDashing)
        {
            return; // If the entity is dashing, do not take damage.
        }
        ShowDamageNumber(Mathf.CeilToInt(amount).ToString());
        health.TakeDamage(amount, isPercentage);
    }

    void ShowDamageNumber(string damage)
    {
        var damageNumber = Instantiate(damageNumberPrefab, transform.position, Quaternion.identity);
        damageNumber.GetComponent<DamageNumber>().SetDamageText($"-{damage}");
    }
}
