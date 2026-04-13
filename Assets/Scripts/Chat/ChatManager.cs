using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public Choice[] choices;
}

[System.Serializable]
public class DialogueList
{
    public Dialogue[] dialogues;
}

public class ChatManager : MonoBehaviour
{
    public TextMeshProUGUI ChatText;
    public TextMeshProUGUI CharacterName;
    public GameObject choicePanel;
    public TextMeshProUGUI[] choiceButtonsText;

    private DialogueList dialogueData;
    private int selectedNextId;

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
        TextAsset jsonData = Resources.Load<TextAsset>("dialogue");
        if (jsonData != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueList>(jsonData.text);
        }
        else
        {
            Debug.Log("제이슨 데이터 확인할 것");
        }
    }

    IEnumerator PlayDialogue()
    {
        int currentId = 0;

        while (currentId != -1)
        {
            Dialogue Line = System.Array.Find(dialogueData.dialogues, d => d.id == currentId);
            if (Line == null) break;

            yield return StartCoroutine(NormalChat(Line.character, Line.text));

            if (Line.choices != null && Line.choices.Length > 0)
            {
                yield return StartCoroutine(ShowChoices(Line.choices));
                currentId = selectedNextId;
            }
            else
            {
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
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                selectedNextId = next;
                choicePanel.SetActive(false);
            });
        }
        yield return new WaitUntil(() => !choicePanel.activeSelf);
    }

    IEnumerator NormalChat(string narrator, string narration)
    {
        CharacterName.text = narrator;
        ChatText.text = "";

        foreach (char letter in narration.ToCharArray())
        {
            ChatText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        // 대사 출력 후 한 프레임을 쉬어야 클릭 감지가 정확해집니다.
        yield return null;

        yield return new WaitUntil(() =>
             (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
             (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame));

        // 클릭 소리가 중복 인식되지 않게 다시 한 프레임 대기
        yield return null;
    }
}