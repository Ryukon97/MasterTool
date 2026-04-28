using UnityEngine;
using System.Collections;

public class LodingScreenController : MonoBehaviour
{
    public GameObject loadingPanel;
    public float displayTime = 1.5f; 
    public float fadeTime = 0.5f;   

    private CanvasGroup canvasGroup;

    void Awake()
    {
       
        canvasGroup = loadingPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = loadingPanel.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        StartCoroutine(StartLoading());
    }

    IEnumerator StartLoading()
    {
        yield return new WaitForSeconds(displayTime);

      
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
            yield return null;
        }

       
        loadingPanel.SetActive(false);
        Debug.Log("<color=lime>마스타! 로딩 완료, 이제 게임을 시작합니다!</color>");
    }


}


