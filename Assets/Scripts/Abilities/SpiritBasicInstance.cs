using UnityEngine;

public class SpiritBasicInstance : AbilityInstance
{
    void Update()
    {
        if (SourceAbility == null) return;
        transform.SetPositionAndRotation(SourceAbility.transform.position, Quaternion.LookRotation(SourceAbility.transform.forward));
    }
    internal override void OnHit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && SourceAbility.PlayerAbility) {
            other.gameObject.GetComponent<Knockback>().KnockbackEnemy(transform.forward, SourceAbility.KnockbackForce, SourceAbility.KnockbackDuration);
        }
    }
}
