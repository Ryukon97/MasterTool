using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public GameObject SettingPanel;

    public static BGMManager instance;
    public AudioClip[] bgmList;

    public AudioSource SourceA;
    public AudioSource SourceB;
    private bool isSourceAActive = true;

    private int currentPlayingIndex = -1;


    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void PlayBGMByIndex(int index, float FadeTime)
    {
        if (currentPlayingIndex == index) return;

        currentPlayingIndex = index;
        AudioClip clip = bgmList[index];
        StartCoroutine(CrossFade(clip, FadeTime));
    }
    IEnumerator CrossFade(AudioClip clip, float Duration)
    {
        AudioSource Active = isSourceAActive ? SourceA : SourceB;
        AudioSource Next = isSourceAActive ? SourceB : SourceA;

        Next.clip = clip;
        Next.volume = 0;
        Next.Play();

        float elapsed = 0;
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / Duration;
            Active.volume = 1 - percent;
            Next.volume = percent;
            yield return null;
        }
        Active.Stop();
        isSourceAActive = !isSourceAActive;
    }
   
}
