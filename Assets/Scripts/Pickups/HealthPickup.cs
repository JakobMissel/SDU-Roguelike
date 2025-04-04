using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] int healAmount = 10;

    protected override void ActivatePickup(GameObject player)
    {
        var playerHealth = player.GetComponentInParent<Health>();
        if (!playerHealth.canBeHealed) return;
        playerHealth.Heal(healAmount);
        base.ActivatePickup(player);
    }
}
