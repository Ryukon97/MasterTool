using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "NewScenario", menuName = "Scenario/DialogueData")]
public class DialogueDataSO : ScriptableObject
{
    public List<DialogueEntry> entries = new List<DialogueEntry>();
}


[System.Serializable]
public class DialogueEntry
{
    public string speakerName;
    [TextArea(3, 10)]
    public string dialogueText;
    public Sprite characterIllust;

    public List<ChoiceData> choices = new List<ChoiceData> ();
}

[System.Serializable]
public class ChoiceData
{
    public string choiceText;
    public int choiceIndex;
}