using UnityEngine;
using UnityEngine.InputSystem;

public class TamerBasicAttack : Ability {
    void Awake() {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update() {
        RunCooldown();        
    }
    public override void ActivateAbility(InputAction.CallbackContext context){
        Debug.Log("hej");
        if (CheckCooldown()) {
            Instantiate(VFX);
            ApplyCooldown();
        }
    }
}
