using UnityEngine;
using UnityEngine.InputSystem;

public class TamerBasicAttack : Ability {
    void Awake() {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update() {
        RunCooldown();
    }

    public override void ActivateAbility(InputAction.CallbackContext context){
        if (CheckCooldown()) {
            animator.Play("Basic");
            var vfx = Instantiate(VFX, transform.position, Quaternion.LookRotation(transform.forward));
            vfx.GetComponent<AbilityInstance>().SetInfo(this);
            ApplyCooldown();
        }
    }
}
