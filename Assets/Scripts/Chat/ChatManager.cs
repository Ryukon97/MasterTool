using System.Collections;
using System.Collections.Generic; // List를 사용하기 위해 필수!
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// 주의: 혹시 상단에 using NUnit.Framework; 가 있다면 삭제해주세요!

public class ChatManager : MonoBehaviour
{
    [Header("Data Source")]
    public DialogueDataSO currentScenario;

    [Header("UI References")]
    public TextMeshProUGUI ChatText;
    public TextMeshProUGUI CharacterName;
    public GameObject choicePanel;
    public TextMeshProUGUI[] choiceButtonsText;

    private int currentIndex = 0;
    public bool isPausedByMenu = false;

    void Start()
    {
        if (currentScenario != null && currentScenario.entries.Count > 0)
        {
            StartCoroutine(PlayDialogue());
        }
    }

    IEnumerator PlayDialogue()
    {
        currentIndex = 0;

        while (currentIndex < currentScenario.entries.Count)
        {
            var entry = currentScenario.entries[currentIndex];

            if (IllustManager.Instance != null && entry.characterIllust != null)
            {
                IllustManager.Instance.ChangeIllust(entry.characterIllust.name);
            }

            // 대사 텍스트 출력
            yield return StartCoroutine(NormalChatOnlyText(entry.speakerName, entry.dialogueText));

            // 선택지 체크
            if (entry.choices != null && entry.choices.Count > 0)
            {
                yield return StartCoroutine(ShowScenarioChoices(entry.choices));
            }
            else
            {
                yield return StartCoroutine(WaitForInput());
                currentIndex++;
            }
        }
        Debug.Log("마스타! 시나리오가 끝났습니다!");
    }

    // --- 여기부터 함수들의 중괄호 { } 배치를 잘 확인해주세요! ---

    IEnumerator ShowScenarioChoices(System.Collections.Generic.List<ChoiceData> choices)
    {
        choicePanel.SetActive(true);
        for (int i = 0; i < choiceButtonsText.Length; i++)
        {
            if (i < choices.Count)
            {
                choiceButtonsText[i].gameObject.transform.parent.gameObject.SetActive(true);
                choiceButtonsText[i].text = choices[i].choiceText;

                int targetIndex = choices[i].choiceIndex;
                Button btn = choiceButtonsText[i].GetComponentInParent<Button>();

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    currentIndex = targetIndex;
                    choicePanel.SetActive(false);
                });
            }
            else
            {
                choiceButtonsText[i].gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
        yield return new WaitUntil(() => !choicePanel.activeSelf);
    } // <-- 함수 종료 중괄호 확인!

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
        yield return new WaitUntil(() =>
        {
            if (isPausedByMenu) return false;
            return (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
                   (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame);
        });
    }
}