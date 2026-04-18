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
    [Header("챕터 id 번호를 꼭 넣어주세요")]
    public int id;
    [Header(" 캐릭터 이름 플레이어 '나'는 자동 묵음 처리됩니다 ")]
    public string speakerName;
    [TextArea(3, 10)]
    public string dialogueText;
    [Header("캐릭터 통 일러스트(1980x1080)")]
    public Sprite characterIllust;
    [Header("캐릭터 전용 PNG(배경투명도 꼭 확인!)")]
    public Sprite CharacterPNG;
    [Header(" 뒷 배경 전용")]
    public Sprite BackGroundSprit;


    [Header("캐릭터 위치확인용")]
    public Vector2 CharacterPos = new Vector2(0, -100);

    [Header(" 선택지 전용칸 해당 id숫자를 넣으면 클릭시 이동합니다")]
    public List<ChoiceData> choices = new List<ChoiceData> ();

    [Header("이 대사 이후 이동할 번호 (기본 값 -1은 순차진행)")]
    public int nextIndexOverride = -1;
}

[System.Serializable]
public class ChoiceData
{
    public string choiceText;
    public int choiceIndex;
}