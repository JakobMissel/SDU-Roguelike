using UnityEngine;
using UnityEngine.InputSystem;

public class BreathAttack : Ability {
    void Awake() {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update() {
        RunCooldown();
    }
    public override void ActivateAbility(InputAction.CallbackContext context){
        if (CheckCooldown()) {
            var vfx = Instantiate(VFX, transform.position, Quaternion.LookRotation(transform.forward));
            // vfx.transform.SetParent(transform);
            vfx.GetComponent<AbilityInstance>().SetInfo(this);
            ApplyCooldown();
        }
    }
}
