using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class EffectPlayManager : MonoBehaviour
{
    public static EffectPlayManager Instance;

    [Header("연결 설정")]
    public VideoPlayer videoPlayer;   
    public GameObject effectContainer; 
    public RawImage displayImage;     

    void Awake()
    {
        Instance = this;
      
        if (effectContainer != null) effectContainer.SetActive(false);
    }

   
    public void PlayVideoEffect(VideoClip clip)
    {
        if (clip == null)
        {
            StopEffect();
            return;
        }

       
        effectContainer.SetActive(true);

        
        videoPlayer.clip = clip;

       
        videoPlayer.Prepare();
        videoPlayer.Play();
    }

    public void StopEffect()
    {
        videoPlayer.Stop();
        if (effectContainer != null) effectContainer.SetActive(false);
    }
}