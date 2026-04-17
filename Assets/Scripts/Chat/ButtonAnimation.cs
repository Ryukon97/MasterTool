using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;

    [Header("설정")]
    public float animationSpeed = 10f; // 크기 변화 속도 (높을수록 빠름)
    public float transitionDelay = 1.4f; // 애니메이션 후 대기 시간
    public SoundEffect soundEffect;

    private Vector3 targetScale;
    private Coroutine transitionRoutine;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
       
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
    }

   
    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = originalScale * 0.9f;

        if(soundEffect !=null)
        {
            soundEffect.PlayClick();
        }
    }

 
    public void OnPointerUp(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

   
    public void ExecuteAfterAnimation(System.Action nextStep)
    {
        if (transitionRoutine != null) StopCoroutine(transitionRoutine);
        transitionRoutine = StartCoroutine(WaitRoutine(nextStep));
    }

    IEnumerator WaitRoutine(System.Action nextStep)
    {
       
        yield return new WaitForSeconds(0.2f);

        
        yield return new WaitForSeconds(transitionDelay);

       
        nextStep?.Invoke();
    }
}