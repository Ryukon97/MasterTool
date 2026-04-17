using UnityEngine;
using System.Collections.Generic;

// [주의] 파일 이름이 반드시 DialogueDataSO.cs 여야 합니다!
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
    public Sprite characterIllust; // JSON의 스트링 대신 직접 스프라이트 연결!
    // 여기에 선택지나 보이스 등을 추가하시면 됩니다.
}