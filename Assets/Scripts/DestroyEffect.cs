using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyEffect: MonoBehaviour {
    [SerializeField] VisualEffect visualEffect;
    [SerializeField] ParticleSystem ParticleSystem;
    
    bool hasPlayed = false;
    private void Awake() {
        if (visualEffect == null) 
            visualEffect = GetComponent<VisualEffect>();
        if (ParticleSystem == null)
            ParticleSystem = GetComponent<ParticleSystem>();
    }

    void Update() {
        if (visualEffect == null)
            goto particleSystem;

        if (!visualEffect.HasAnySystemAwake() && hasPlayed) { 
            Destroy();
        }
        else if (visualEffect.HasAnySystemAwake() && !hasPlayed) {
            hasPlayed = true;
        }
        particleSystem:
        if (ParticleSystem == null)
            return;
        if (!ParticleSystem.isPlaying && hasPlayed) {
            Destroy(gameObject);
        }
        else if (ParticleSystem.isPlaying && !hasPlayed) {
            hasPlayed = true;
        }
    }

    void Destroy() {
        Destroy(gameObject);
    }
}
