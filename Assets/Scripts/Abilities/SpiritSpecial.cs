using UnityEngine;
using UnityEngine.InputSystem;

public class SpiritSpecial : Ability {
    void Awake() {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update() {
        RunCooldown();
    }
}
