using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip clickSound;

    public void SetVolume(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value;
        }
    }

    public void PlayClick()
    {
        if (audioSource != null && clickSound != null)
        {
          
            audioSource.PlayOneShot(clickSound);
        }
    }
}
