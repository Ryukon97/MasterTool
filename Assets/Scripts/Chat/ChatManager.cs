using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ChatManager : MonoBehaviour
{
    [Header("Data Source")]
    public DialogueDataSO currentScenario;
    public SoundDataSO bgmSetting;

    [Header("UI References")]
    public TextMeshProUGUI ChatText;
    public TextMeshProUGUI CharacterName;
    public GameObject choicePanel;
    public TextMeshProUGUI[] choiceButtonsText;
    public Image CharacterImage;
    public Image BackgroundImage;
    public Image EffectImage;

    private Coroutine RotationCoroutine;
    private Coroutine CharacterMoveCoroutine;
    private DialogueEntry currentEntry;
    public bool isPausedByMenu = false;
    private int nextIDResult = -1;

    void Start()
    {
        if (currentScenario != null && currentScenario.groups.Count > 0)
        {
            int firstID = currentScenario.groups[0].entries[0].id;
            StartCoroutine(PlayDialogue(firstID));
        }
    }

    public IEnumerator PlayDialogue(int startID)
    {
        //int currentGroupIdx = 0;
        //currentEntry = currentScenario.groups[currentGroupIdx].entries.Find(x => x.id == startID);

        DialogueEntry EntryToPlay = null;

       
        foreach (var group in currentScenario.groups)
        {
            EntryToPlay = group.entries.Find(x => x.id == startID);
            if (EntryToPlay != null)
                break; 
        }

        currentEntry = EntryToPlay;

        while (currentEntry != null)
        {

            CheckBGMEvent(currentEntry.id);

            if (currentEntry.EffectSound != null && BGMManager.instance != null)
            {

                BGMManager.instance.PlayOneShotSE(currentEntry.EffectSound, currentEntry.seVolune);
            }
            if (BackgroundImage != null)
            {
                if (currentEntry.BackGroundSprit != null)
                {
                    BackgroundImage.gameObject.SetActive(true);
                    BackgroundImage.sprite = currentEntry.BackGroundSprit;
                }

            }


            if (CharacterImage != null) // 캐릭터 관련 값조절하는 거
            {
                if (currentEntry.CharacterPNG != null)
                {
                    CharacterImage.gameObject.SetActive(true);
                    CharacterImage.sprite = currentEntry.CharacterPNG;
                    CharacterImage.SetNativeSize();

                    CharacterImage.rectTransform.localScale = Vector3.one*currentEntry.CharacterScale; //캐릭터 스케일값
                    //CharacterImage.rectTransform.localRotation = Quaternion.Euler(0, 0, currentEntry.CharacterRotation);

                    if (CharacterMoveCoroutine != null) StopCoroutine(CharacterMoveCoroutine); // XY축으로 움직이는거
                    CharacterMoveCoroutine = StartCoroutine(AnimateCharacter(currentEntry.CharacterPos, currentEntry.moveDuration));

                    if (RotationCoroutine != null) StopCoroutine(RotationCoroutine); //Z축 회전
                    RotationCoroutine = StartCoroutine(AnimationRotation(currentEntry.CharacterRotation, currentEntry.moveDuration));
                }
                else if (currentEntry.characterIllust != null)
                {
                    CharacterImage.gameObject.SetActive(true);
                    CharacterImage.sprite = currentEntry.characterIllust;


                    CharacterImage.rectTransform.anchoredPosition = Vector2.zero;
                    CharacterImage.rectTransform.localRotation = Quaternion.identity;
                }
                else
                {
                    CharacterImage.gameObject.SetActive(false);
                }
            }


            if (EffectImage != null) // 이펙트 관련
            {
                if (currentEntry.EffectSprite != null)
                {
                    EffectImage.gameObject.SetActive(true);
                    EffectImage.sprite = currentEntry.EffectSprite;
                    EffectImage.rectTransform.anchoredPosition = currentEntry.EffectPos;
                    EffectImage.rectTransform.localScale = Vector3.one * currentEntry.EffectScale;
                    EffectImage.SetNativeSize();
                }
                else
                {
                    EffectImage.gameObject.SetActive(false);
                }
            }


            yield return StartCoroutine(NormalChatOnlyText(currentEntry.speakerName, currentEntry.dialogueText));
            yield return StartCoroutine(WaitForInput());


            int nextID = -1;
            if (currentEntry.choices != null && currentEntry.choices.Count > 0)
            {
                yield return StartCoroutine(ShowScenarioChoices(currentEntry.choices));
                nextID = nextIDResult;
            }
            else if (currentEntry.nextIndexOverride != -1)
            {
                nextID = currentEntry.nextIndexOverride;
            }
            else
            {
                nextID = currentEntry.id + 1;
            }


            //currentEntry = currentScenario.groups[currentGroupIdx].entries.Find(x => x.id == nextID);
            DialogueEntry NextfoundEntry = null;


            foreach (var group in currentScenario.groups)
            {
                NextfoundEntry = group.entries.Find(x => x.id == nextID);
               
                if (NextfoundEntry != null) break;
               
            }

            currentEntry = NextfoundEntry;

            if (currentEntry == null)
            {
                if (CharacterImage != null) CharacterImage.gameObject.SetActive(false);
                Debug.Log("<color=yellow>시나리오가 끝났습니다!</color>");

                ScenarioController controller = FindAnyObjectByType<ScenarioController>();
                if (controller != null)
                {
                    controller.EndOfDialogue();
                }
                break;
            }
        }
    }



    IEnumerator AnimateCharacter(Vector2 TargetPos, float duration)
    {
        RectTransform rect = CharacterImage.rectTransform;
        Vector2 startPos = rect.anchoredPosition;
        float elapsed = 0f;


        if (duration <= 0f)
        {
            rect.anchoredPosition = TargetPos;
            yield break;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(startPos, TargetPos, elapsed / duration);
            yield return null;
        }
        rect.anchoredPosition = TargetPos;
    }

    IEnumerator AnimationRotation(float TragetZRotation, float Duration)
    {
        RectTransform rect = CharacterImage.rectTransform;
        Quaternion StartRot = rect.localRotation;
        Quaternion TargetRot = Quaternion.Euler(0, 0, TragetZRotation);
        float elapsed = 0f;

        if (Duration <= 0f)
        {
            rect.localRotation = TargetRot;
            yield break;
        }
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            rect.localRotation = Quaternion.Lerp(StartRot, TargetRot, elapsed / Duration);
            yield return null;
        }
        rect.localRotation = TargetRot;
    }

    IEnumerator ShowScenarioChoices(List<ChoiceData> choices)
    {
        nextIDResult = -1;
        choicePanel.SetActive(true);

        for (int i = 0; i < choiceButtonsText.Length; i++)
        {
            if (i < choices.Count)
            {
                choiceButtonsText[i].gameObject.transform.parent.gameObject.SetActive(true);
                choiceButtonsText[i].text = choices[i].choiceText;

                int targetID = choices[i].choiceIndex;
                Button btn = choiceButtonsText[i].GetComponentInParent<Button>();

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    StartCoroutine(OnchoieClicked(targetID));
                });
            }
            else
            {
                choiceButtonsText[i].gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
        yield return new WaitUntil(() => nextIDResult != -1);
    }

    IEnumerator OnchoieClicked(int targetID)
    {
        yield return new WaitForSecondsRealtime(0.15f);
        nextIDResult = targetID;
        choicePanel.SetActive(false);
    }

    IEnumerator NormalChatOnlyText(string narrator, string narration)
    {
        CharacterName.text = (narrator == "나") ? " " : narrator;
        ChatText.text = "";

        foreach (char letter in narration.ToCharArray())
        {
            if (isPausedByMenu) yield return new WaitUntil(() => !isPausedByMenu);
            ChatText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator WaitForInput()
    {
        yield return new WaitForSeconds(0.1f);
        bool cliked = false;
        while (!cliked)
        {
            if(isPausedByMenu)
            {
                yield return null;
                continue;

            }
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) cliked = true;
            if(Keyboard.current !=null && Keyboard.current.anyKey.wasPressedThisFrame)cliked = true;
            //if(Input.GetButtonDown(0)) cliked = true;

            yield return null;
        }
        Debug.Log("<color=white>입력 감지됨: 다음 대사로 진행합니다.</color>");
        //yield return new WaitUntil(() =>
        //{
        //    if (isPausedByMenu) return false;
        //    return (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
        //           (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame);
        //});
    }
   


    void CheckBGMEvent(int currentID)
    {

        var bgmEvent = bgmSetting.BGMEvents.Find(e => currentID >= e.StartID && currentID <= e.EndID);
        if (bgmEvent != null)
        {
            BGMManager.instance.CheckAndPlayBGM(currentID);
        }
    }
}