using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// --- 데이터 구조 클래스들 (에러 발생 원인 해결) ---
[System.Serializable]
public class Choice
{
    public string text;
    public int nextId;
}

[System.Serializable]
public class Dialogue
{
    public int id;
    public string character;
    public string text;
    public int nextId;
    public string illustName;
    public Choice[] choices;
}

[System.Serializable]
public class DialogueList
{
    public Dialogue[] dialogues;
}
// ---------------------------------------------

public class ChatManager : MonoBehaviour
{
    public TextMeshProUGUI ChatText;
    public TextMeshProUGUI CharacterName;
    public GameObject choicePanel;
    public TextMeshProUGUI[] choiceButtonsText;

    private DialogueList dialogueData;
    private int selectedNextId;
    public bool isPausedByMenu = false;
    public SettingManager settingManager;

    void Start()
    {
        LoadDialogue();
        if (dialogueData != null && dialogueData.dialogues.Length > 0)
        {
            StartCoroutine(PlayDialogue());
        }
    }

    void LoadDialogue()
    {
        // Resources/dialogue.json 파일을 로드합니다.
        TextAsset jsonData = Resources.Load<TextAsset>("dialogue");
        if (jsonData != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueList>(jsonData.text);
        }
        else
        {
            Debug.LogError("마스타! Resources 폴더에 dialogue 제이슨 파일이 있는지 확인해 주세요!");
        }
    }

    IEnumerator PlayDialogue()
    {
        int currentId = 0;

        while (currentId != -1)
        {
            Dialogue Line = System.Array.Find(dialogueData.dialogues, d => d.id == currentId);
            if (Line == null) break;

            if (IllustManager.Instance != null)
            {
                IllustManager.Instance.ChangeIllust(Line.illustName);
            }

            // 대사 텍스트 출력
            yield return StartCoroutine(NormalChatOnlyText(Line.character, Line.text));

            if (Line.choices != null && Line.choices.Length > 0)
            {
                // 선택지가 있는 경우: 바로 선택지 창 활성화
                yield return StartCoroutine(ShowChoices(Line.choices));
                currentId = selectedNextId;
            }
            else
            {
                // 선택지가 없는 경우: 클릭 입력 대기 후 다음으로
                yield return StartCoroutine(WaitForInput());
                currentId = Line.nextId;
            }
        }
    }

    IEnumerator ShowChoices(Choice[] choices)
    {
        choicePanel.SetActive(true);

        for (int i = 0; i < choices.Length; i++)
        {
            if (i >= choiceButtonsText.Length) break;
            choiceButtonsText[i].text = choices[i].text;

            int next = choices[i].nextId;
            Button btn = choiceButtonsText[i].GetComponentInParent<Button>();

            // 우리가 만든 스크립트 가져오기
            var btnAnim = btn.GetComponent<ButtonAnimation>();

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => {
                // 중복 클릭 방지
                btn.interactable = false;

                if (btnAnim != null)
                {
                    // 애니메이션과 1.4초 대기 후 실행될 로직 전달
                    btnAnim.ExecuteAfterAnimation(() => {
                        selectedNextId = next;
                        choicePanel.SetActive(false);
                    });
                }
                else
                {
                    // 스크립트가 없다면 예외처리로 즉시 이동
                    selectedNextId = next;
                    choicePanel.SetActive(false);
                }
            });
        }

        yield return new WaitUntil(() => !choicePanel.activeSelf);
    }

    IEnumerator NormalChatOnlyText(string narrator, string narration)
    {
        CharacterName.text = (narrator == "나") ? " " : narrator;
        ChatText.text = "";
        foreach (char letter in narration.ToCharArray())
        {
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