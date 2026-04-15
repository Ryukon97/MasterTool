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
        // 매 프레임 타겟 크기로 부드럽게 이동 (속도 조절 가능!)
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
    }

    // 마우스를 누르는 순간 (Touch!)
    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = originalScale * 0.9f;

        if(soundEffect !=null)
        {
            soundEffect.PlayClick();
        }
    }

    // 마우스에서 손을 떼는 순간
    public void OnPointerUp(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

    // ⭐ ChatManager에서 이 함수를 호출하게 하세요!
    public void ExecuteAfterAnimation(System.Action nextStep)
    {
        if (transitionRoutine != null) StopCoroutine(transitionRoutine);
        transitionRoutine = StartCoroutine(WaitRoutine(nextStep));
    }

    IEnumerator WaitRoutine(System.Action nextStep)
    {
        // 1. 버튼이 원래 크기로 돌아올 때까지 아주 잠깐 대기 (0.1~0.2초)
        yield return new WaitForSeconds(0.2f);

        // 2. 마스타가 명하신 1.4초의 대기 시간
        yield return new WaitForSeconds(transitionDelay);

        // 3. 이제 다음 경로로!
        nextStep?.Invoke();
    }
}