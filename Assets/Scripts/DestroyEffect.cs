using UnityEngine;
using UnityEngine.VFX;

public class DestroyEffect: MonoBehaviour
{
    VisualEffect visualEffect;
    bool hasPlayed = false;
    private void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    void Update()
    {
        if (!visualEffect.HasAnySystemAwake() && hasPlayed)
        { 
            Destroy();
        }
        else if (visualEffect.HasAnySystemAwake() && !hasPlayed)
        {
            hasPlayed = true;
        }
    }

    void Destroy() 
    {
        Destroy(gameObject);
    }
}
