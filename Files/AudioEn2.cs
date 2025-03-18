using UnityEngine;

public class AudioEn2 : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioClip != null)
        {
            Invoke("AudioPlay", 0.5f);
        }
    }

    private void AudioPlay()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
