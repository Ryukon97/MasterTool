using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("Data Source")]
    public DialogueDataSO currentScenario;

    [Header("UI References")]
    public TextMeshProUGUI ChatText;
    public TextMeshProUGUI CharacterName;
    public GameObject choicePanel;
    public TextMeshProUGUI[] choiceButtonsText;
    public Image CharacterImage;

    private DialogueEntry currentEntry;
    public bool isPausedByMenu = false;
    private int nextIDResult = -1;

    void Start()
    {

        if (currentScenario != null && currentScenario.entries.Count > 0)
        {

            int firstID = currentScenario.entries[0].id;
            StartCoroutine(PlayDialogue(firstID));
        }
    }

    IEnumerator PlayDialogue(int startID)
    {
        currentEntry = currentScenario.entries.Find(x => x.id == startID);

        while (currentEntry != null)
        {
           
            if (CharacterImage != null)
            {
                if (currentEntry.characterIllust != null)
                {
                    CharacterImage.gameObject.SetActive(true);
                    CharacterImage.sprite = currentEntry.characterIllust;
                }
                else 
                {
                    CharacterImage.gameObject.SetActive(false);
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

            currentEntry = currentScenario.entries.Find(x => x.id == nextID);

            if (currentEntry == null)
            {
             
                if (CharacterImage != null) CharacterImage.gameObject.SetActive(false);
                Debug.Log("<color=yellow>마스타! 시나리오가 끝났습니다!</color>");
                break;
            }
        }
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
        yield return new WaitUntil(() =>
        {
            if (isPausedByMenu) return false;
            return (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
                   (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame);
        });
    }
}