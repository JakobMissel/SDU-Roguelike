using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Ability : MonoBehaviour {
    [SerializeField] internal Image CooldownImage;
    [SerializeField] internal string CooldownImageName;
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
    [SerializeField] internal float AreaModifier;

    [SerializeField] internal float KnockbackForce;
    [SerializeField] internal float KnockbackDuration;

    [SerializeField] internal GameObject VFX;

    [SerializeField] internal PlayerInput playerInput;
    [SerializeField] internal Animator animator;
    [SerializeField] internal string CastAnimation;
    [SerializeField] internal string ActivateActionName;

    [SerializeField] internal float ProjectileSpeed;
    [SerializeField] internal bool PlayerAbility;
    void Awake() {
        if (!PlayerAbility) return;
        playerInput = GetComponent<PlayerInput>();
    }
    public void GetCooldownImage() {
        CooldownImage = GameObject.Find(CooldownImageName)?.GetComponent<Image>();
    }
    protected void OnEnable()
    {
        if (!PlayerAbility) return;
        if (!string.IsNullOrEmpty(ActivateActionName))
        {
            var action = playerInput.actions[ActivateActionName];
            if (action != null)
            {
                action.started += ActivateAbility;
            }
            else
            {
                Debug.LogError($"Action '{ActivateActionName}' not found!");
            }
        }

        GameEvents.OnPlayerDeath += OnDisable;
    }

    protected void OnDisable() {
        if (!PlayerAbility) return;
        if (!string.IsNullOrEmpty(ActivateActionName))
            playerInput.actions[ActivateActionName].started -= ActivateAbility;

        GameEvents.OnPlayerDeath -= OnDisable;
    }

    public virtual void ActivateAbility(InputAction.CallbackContext context){
        if (CanCast()) {
            CurrentCastTime = CastTime;
            ApplyCooldown();
            animator.Play(CastAnimation);
            Invoke("CastAbility", CastTime);
            // var vfx = Instantiate(VFX, transform.position, Quaternion.LookRotation(transform.forward));
            // vfx.GetComponent<AbilityInstance>().SetInfo(this);
            // ApplyCooldown();
        }
    }
    public virtual void ActivateAbility(){
        if (CanCast()) {
            CurrentCastTime = CastTime;
            ApplyCooldown();
            animator.Play(CastAnimation);
            Invoke("CastAbility", CastTime);
        }
    }
    internal void RunCooldown(){
        if (CurrentCastTime <= 0)
        {
            CurrentCooldown = Mathf.Clamp(CurrentCooldown - Time.deltaTime, 0, Cooldown);
        }
        else
        {
            CurrentCastTime -= Time.deltaTime;
        }
        if (CooldownImage != null) {
            CooldownImage.fillAmount = Mathf.Clamp01(CurrentCooldown / Cooldown);
        }
    }
    public bool CanCast(){
        if (CheckCooldown() && CurrentCastTime <= 0){
            return true;
        }
        return false;
    }
    public virtual void CastAbility(){
            var vfx = Instantiate(VFX, transform.position, Quaternion.LookRotation(transform.forward));
            vfx.transform.localScale = Vector3.one * AreaModifier;
            vfx.GetComponent<AbilityInstance>().SetInfo(this);
    }

    public bool CheckCooldown() {
        if (CurrentCooldown > 0) {
            return false;
        }
        return true;
    }

    public virtual void ApplyCooldown(){
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
    public int CalculateDamage(){
        return (int)(Damage+(float)Damage*DamageModifier);
    }
}
