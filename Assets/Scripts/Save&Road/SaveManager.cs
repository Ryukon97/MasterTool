using NUnit.Framework.Constraints;
using System.Collections; //start Coroutine; ->이거쓸려면 써야함
using System.Drawing;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using Unity.VisualScripting;
using UnityEditor.Overlays;
#endif
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{

    private string SavePath;

    private string GetSavePath(int index)
    {
        return Path.Combine(Application.persistentDataPath, $"save_slot_{index}.json");
    }

    public void SaveGame(int index) // 세이브 버튼에 연결할 onclick함수

    {

        ChatManager cm = FindAnyObjectByType<ChatManager>();//ChatManager ->cm으로 바꿔서 가독성추가
        if (cm == null)
        {
            Debug.LogError("ChatManager를 찾을 수 없습니다!");
            return;
        }
        SaveAndLoad Data = new SaveAndLoad();

        Data.SaveToday = System.DateTime.Now.ToString("yyyy.MM.dd./HH:mm"); // 이거 넣으면 날짜시간나옴
        if (cm.currentScenario != null)
        {
            Data.SOdataName = cm.currentScenario.name;
        }
        else
        {
           
            Data.SOdataName = cm.currentEntry.speakerName + " - " + cm.currentEntry.id;
        }

        Data.CurrentID = cm.currentEntry.id;
        Data.SpeakerName = cm.currentEntry.speakerName;
        Data.DialogueText = cm.currentEntry.dialogueText;

        if (cm.CharacterImage != null) //캐릭터 저장칸
        {
            Data.Char1Pos = cm.CharacterImage.rectTransform.anchoredPosition;
            Data.Char1Scale = cm.CharacterImage.rectTransform.localScale.x;
            Data.Char1Rotation = cm.CharacterImage.rectTransform.localRotation.eulerAngles.z;
        }
        if (cm.CharacterImage != null) //캐릭터 저장칸
        {
            Data.Char2Pos = cm.CharacterImage2.rectTransform.anchoredPosition;
            Data.Char2Scale = cm.CharacterImage2.rectTransform.localScale.x;
            Data.Char2Rotation = cm.CharacterImage2.rectTransform.localRotation.eulerAngles.z;
        }
        if (cm.BackgroundImage != null && cm.BackgroundImage.sprite != null)
            Data.BackgroundSpriteName = cm.BackgroundImage.sprite.name; //배경 및 브금 저정칸

        Data.MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        Data.Brightness = PlayerPrefs.GetFloat("Brightness", 1f);


        string path = GetSavePath(index);
        string Json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(path, Json);
        //File.WriteAllText(path, Json); // 1번슬롯에 저장
        //File.WriteAllText(SavePath, Json); //2번 슬롯에 저장
        //Debug.Log($"<color=cyan>슬롯 {index} 저장 완료!</color> 경로: {path}");
        try
        {
            File.WriteAllText(path, Json);
            Debug.Log($"<color=cyan>슬롯 {index} 저장 성공!</color> 장면: {Data.SOdataName}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"저장 실패: {e.Message}");
        }

    }
    public void LoadGame(int index) //불러오기 전용 데이터 여기도 chatManager->cm으로 변경
    {

        string path = GetSavePath(index);

        if (!File.Exists(path)) //! = 파일이 없는경우 
        {
            Debug.LogWarning("저장된 파일이 없습니다!");
            return;
        }

        string json = File.ReadAllText(path);

        ChatManager cm = FindAnyObjectByType<ChatManager>();
        if (cm == null) return;

        //string json = File.ReadAllText(SavePath);
        SaveAndLoad Data = JsonUtility.FromJson<SaveAndLoad>(json);


        cm.StopAllCoroutines();
        Data.MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f); // 사운드 저장값


        // [저장 시]
        Data.Char1Pos = cm.CharacterImage.rectTransform.anchoredPosition;
        Data.Char1Scale = cm.CharacterImage.rectTransform.localScale.x;
        Data.isChar1Active = cm.CharacterImage.gameObject.activeSelf;

        Data.Char2Pos = cm.CharacterImage2.rectTransform.anchoredPosition;
        Data.Char2Scale = cm.CharacterImage2.rectTransform.localScale.x;
        Data.isChar2Active = cm.CharacterImage2.gameObject.activeSelf;

        // [불러오기 시]
        // 메인 캐릭터 복구
        cm.CharacterImage.gameObject.SetActive(Data.isChar1Active);
        cm.CharacterImage.rectTransform.anchoredPosition = Data.Char1Pos;
        cm.CharacterImage.rectTransform.localScale = UnityEngine.Vector3.one * Data.Char1Scale;

        // 서브 캐릭터 복구
        cm.CharacterImage2.gameObject.SetActive(Data.isChar2Active);
        cm.CharacterImage2.rectTransform.anchoredPosition = Data.Char2Pos;
        cm.CharacterImage2.rectTransform.localScale = UnityEngine.Vector3.one * Data.Char2Scale;

        cm.StartCoroutine(cm.PlayDialogue(Data.CurrentID)); // 대화시작
    }

    public void DeletGame(int SlotIndex)
    {
        string path = Path.Combine(Application.persistentDataPath, $"save_slot_{SlotIndex}.json");

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"<color=red>{SlotIndex}번 슬롯 데이터 삭제 완료!</color>");
        }
        else
        {
            Debug.LogWarning($"{SlotIndex}번 파일이 존재하지 않아 삭제할 수 없습니다.");
        }

    }

}



