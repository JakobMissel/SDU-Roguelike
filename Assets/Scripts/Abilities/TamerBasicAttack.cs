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

    public override void CastAbility() {
            var vfx = Instantiate(VFX, transform.position, Quaternion.LookRotation(transform.forward));
            vfx.GetComponent<AbilityInstance>().SetInfo(this);
    }
}
