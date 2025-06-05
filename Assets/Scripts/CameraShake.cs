using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeAmplitude = 1f; 
    [SerializeField] float shakeFrequency = 2f; 

    CinemachineBasicMultiChannelPerlin noise;
    bool isShaking = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        noise = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float duration = 0.5f, float amplitude = 1, float frequency = 5)
    {
        shakeDuration = duration;
        shakeAmplitude = amplitude;
        shakeFrequency = frequency;
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }

    IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        
        float originalAmplitude = noise.AmplitudeGain;
        float originalFrequency = noise.FrequencyGain;

        noise.AmplitudeGain = shakeAmplitude;
        noise.FrequencyGain = shakeFrequency;

        float fadeOutTimeElapsed = 0f;
        while (fadeOutTimeElapsed <= shakeDuration)
        {
            noise.AmplitudeGain = Mathf.Lerp(noise.AmplitudeGain, originalAmplitude, fadeOutTimeElapsed / shakeDuration);
            noise.FrequencyGain = Mathf.Lerp(noise.FrequencyGain, originalFrequency, fadeOutTimeElapsed / shakeDuration);

            fadeOutTimeElapsed += Time.deltaTime;
            yield return null;
        }
        noise.AmplitudeGain = originalAmplitude;
        noise.FrequencyGain = originalFrequency;

        isShaking = false;
    }
}
