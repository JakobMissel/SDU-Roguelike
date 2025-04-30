using UnityEngine;

public class PlaySoundEffect : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip clip;

    void Awake()
    {
        if(audioSource != null)
            audioSource = GetComponent<AudioSource>();
        else
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
