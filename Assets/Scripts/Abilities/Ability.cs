using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability : MonoBehaviour {
    [SerializeField] internal float Cooldown;
    [SerializeField] internal float CooldownModifier;
    [SerializeField] internal float CurrentCooldown;
    [SerializeField] internal float CastTime;
    [SerializeField] internal float CurrentCastTime;
    [SerializeField] internal bool IsCasting;
    
    [SerializeField] internal float Damage;
    [SerializeField] internal float DamageModifier;

    [SerializeField] internal float Range;
    [SerializeField] internal float RangeModifier;

    [SerializeField] internal GameObject HitBox;
    [SerializeField] internal float AreaModifier;
    [SerializeField] internal GameObject VFX;

    [SerializeField] internal PlayerInput playerInput;
    [SerializeField] internal Animator animator;
    // [SerializeField] internal InputActionReference Activate;
    [SerializeField] internal string ActivateActionName;

    // void Awake() {
    //     playerInput = GetComponent<PlayerInput>();
    // }
    protected void OnEnable() {
        if (ActivateActionName != "")
            playerInput.actions[ActivateActionName].started += ActivateAbility;
        // Activate.action.performed += ActivateAbility;
    }

    protected void OnDisable() {
        if (ActivateActionName != "")
            playerInput.actions[ActivateActionName].started -= ActivateAbility;
        // Activate.action.performed -= ActivateAbility;
        // Activate.action.Disable();
    }

    public virtual void ActivateAbility(InputAction.CallbackContext context){}
    public virtual void ActivateAbility(){}
    internal void RunCooldown(){
        CurrentCooldown = Mathf.Clamp(CurrentCooldown - Time.deltaTime, 0, Cooldown);
        // if (CurrentCooldown < 0) {
        //     CurrentCooldown = 0;
        //     return;
        // }
        // if (CurrentCooldown > 0)
        //     CurrentCooldown -= Time.deltaTime;
    }

    public bool CheckCooldown() {
        if (CurrentCooldown > 0) {
            return false;
        }
        return true;
    }

    internal void ApplyCooldown(){
        CurrentCooldown = Cooldown*(1-CooldownModifier);
    }

    public void EditModifier(Modifier.AbilityModifier type, float change){
        switch (type) {
            case Modifier.AbilityModifier.Cooldown:
                CooldownModifier += change;
                break;
            case Modifier.AbilityModifier.Damage:
                DamageModifier += change;
                break;
            case Modifier.AbilityModifier.Range:
                Range += change;
                break;
            case Modifier.AbilityModifier.Area:
                AreaModifier += change;
                break;
            default:
                break;
        }
    }
}
