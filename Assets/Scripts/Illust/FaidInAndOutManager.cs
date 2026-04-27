using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FaidInAndOutManager : MonoBehaviour
{
    public Image backgroundImage; // 배경의 수치값을 받기위해 
    public Image OverlayPanel; // 검정과 하얀패널

    public IEnumerator FadeBackGround(float TartgetAlpha, float Duration) // 기초 페이드 인아웃 조절
    {
        float StartAlpha = backgroundImage.color.a;
        float time = 0; // 대문자를 쓰려했으나 아래 델타타임 때문에 소문자 t씀

        while (time < Duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(StartAlpha, TartgetAlpha, time / Duration);
            backgroundImage.color = new Color(backgroundImage.color.r,backgroundImage.color.g,backgroundImage.color.b,alpha);
            yield return null;

        }
        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, TartgetAlpha);
    }

    // 영상편집의 검정과 하얀색 물들이기를 활용 응용한 버전
    public IEnumerator ColorFade (Color TartgetColor, float Duration,bool IsOut)
    {
        OverlayPanel.color = new Color(TartgetColor.r, TartgetColor.g, TartgetColor.b, IsOut ? 0 : 1);
        float StartAlpha = IsOut ? 0 : 1;
        float EndAlpha = IsOut ? 1 : 0;

        float time = 0;
        while (time < Duration)
        {
           time += Time.deltaTime;
            float Alpha = Mathf.Lerp(StartAlpha,EndAlpha , time / Duration);
            OverlayPanel.color = new Color(TartgetColor.r,TartgetColor.g,TartgetColor.b, Alpha);
            yield return null;
        }
    }

}
