using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyEffect: MonoBehaviour {
    [SerializeField] VisualEffect visualEffect;
    bool hasPlayed = false;
    private void Awake() {
        if (visualEffect == null) 
            visualEffect = GetComponent<VisualEffect>();
    }

    void Update() {
        if (!visualEffect.HasAnySystemAwake() && hasPlayed) { 
            Destroy();
        }
        else if (visualEffect.HasAnySystemAwake() && !hasPlayed) {
            hasPlayed = true;
        }
    }

    void Destroy() {
        Destroy(gameObject);
    }
}
