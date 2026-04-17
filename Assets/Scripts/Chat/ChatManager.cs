using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("Data Source")]
    // JSON 대신 우리가 만든 SO를 연결합니다!
    public DialogueDataSO currentScenario;

    [Header("UI References")]
    public TextMeshProUGUI ChatText;
    public TextMeshProUGUI CharacterName;
    public GameObject choicePanel;
    public TextMeshProUGUI[] choiceButtonsText;

    private int currentIndex = 0; // 이제 ID 대신 리스트의 Index를 사용합니다.
    public bool isPausedByMenu = false;
    public SettingManager settingManager;

    void Start()
    {
        // 로딩 과정이 필요 없습니다! 바로 시작합니다.
        if (currentScenario != null && currentScenario.entries.Count > 0)
        {
            StartCoroutine(PlayDialogue());
        }
        else
        {
            Debug.LogError("마스타! 인스펙터에서 Scenario Data를 연결했는지 확인해 주세요!");
        }
    }

    IEnumerator PlayDialogue()
    {
        currentIndex = 0;

        // 리스트의 끝에 도달할 때까지 반복합니다.
        while (currentIndex < currentScenario.entries.Count)
        {
            var entry = currentScenario.entries[currentIndex];

            // 일러스트 변경 (SO에 연결된 스프라이트를 직접 전달)
            if (IllustManager.Instance != null && entry.characterIllust != null)
            {
                // IllustManager의 ChangeIllust가 Sprite를 받도록 수정되거나, 
                // 기존처럼 이름을 쓰려면 entry.characterIllust.name을 전달하세요.
                IllustManager.Instance.ChangeIllust(entry.characterIllust.name);
            }

            // 대사 텍스트 출력
            yield return StartCoroutine(NormalChatOnlyText(entry.speakerName, entry.dialogueText));

            // [참고] 현재 마스타의 SO 구조에는 선택지가 DialogueEntry 안에 아직 없으므로 
            // 일단은 클릭 입력 대기 후 다음 인덱스로 넘어가게 구성합니다.
            yield return StartCoroutine(WaitForInput());
            currentIndex++;
        }

        Debug.Log("마스타! 챕터 1 데모 분량이 끝났습니다!");
    }

    IEnumerator NormalChatOnlyText(string narrator, string narration)
    {
        // "나"인 경우 이름을 비워두는 마스타의 센스 유지!
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
        // 마스타가 만드신 입력 대기 로직 유지
        yield return new WaitUntil(() =>
        {
            if (isPausedByMenu) return false;
            return (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
                   (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame);
        });
    }
}