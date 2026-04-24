using NUnit.Framework.Constraints;
using System.Drawing;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;
using System.Collections; //start Coroutine; ->이거쓸려면 써야함

public class SaveManager
{
    private string SavePath;

    void Awake()
    {
        SavePath = Path.Combine(Application.persistentDataPath, "SaveFile.Json");

    }

    public void SaveGame(ChatManager chatManager) // 세이브 버튼에 연결할 onclick함수

    {
        SaveAndLoad Data = new SaveAndLoad();

        Data.CurrentID = chatManager.currentEntry.id;
        Data.SpeakerName = chatManager.currentEntry.speakerName;
        Data.DialogueText = chatManager.currentEntry.dialogueText;

        if (chatManager.CharacterImage != null) //캐릭터 저장칸
        {
            Data.Char1Pos = chatManager.CharacterImage.rectTransform.anchoredPosition;
            Data.Char1Scale = chatManager.CharacterImage.rectTransform.localScale.x;
            Data.Char1Rotation = chatManager.CharacterImage.rectTransform.localRotation.eulerAngles.z;
        }
        if (chatManager.BackgroundImage != null && chatManager.BackgroundImage.sprite != null)
            Data.BackgroundSpriteName = chatManager.BackgroundImage.sprite.name; //배경 및 브금 저정칸

        Data.MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        Data.Brightness = PlayerPrefs.GetFloat("Brightness", 1f);



            string Json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(SavePath, Json);
        Debug.Log ("< color = cyan > 저장 완료! </ color > 경로: " + SavePath);

    }
    public void LoadGame(ChatManager chatManager) //불러오기 전용 데이터
    {
        if (!File.Exists(SavePath)) //! = 파일이 없는경우 
        {
            Debug.LogWarning("저장된 파일이 없습니다!");
            return;
        }
        
            string json = File.ReadAllText(SavePath);
           SaveAndLoad Data = JsonUtility.FromJson<SaveAndLoad>(json);

        chatManager.StopAllCoroutines();
        chatManager.StartCoroutine(chatManager.PlayDialogue(Data.CurrentID));

        // [저장 시]
        Data.Char1Pos = chatManager.CharacterImage.rectTransform.anchoredPosition;
        Data.Char1Scale = chatManager.CharacterImage.rectTransform.localScale.x;
        Data.isChar1Active = chatManager.CharacterImage.gameObject.activeSelf;

        Data.Char2Pos = chatManager.CharacterImage2.rectTransform.anchoredPosition;
        Data.Char2Scale = chatManager.CharacterImage2.rectTransform.localScale.x;
        Data.isChar2Active = chatManager.CharacterImage2.gameObject.activeSelf;

        // [불러오기 시]
        // 메인 캐릭터 복구
        chatManager.CharacterImage.gameObject.SetActive(Data.isChar1Active);
        chatManager.CharacterImage.rectTransform.anchoredPosition = Data.Char1Pos;
        chatManager.CharacterImage.rectTransform.localScale = UnityEngine.Vector3.one * Data.Char1Scale;

        // 서브 캐릭터 복구
        chatManager.CharacterImage2.gameObject.SetActive(Data.isChar2Active);
        chatManager.CharacterImage2.rectTransform.anchoredPosition = Data.Char2Pos;
        chatManager.CharacterImage2.rectTransform.localScale = UnityEngine.Vector3.one * Data.Char2Scale;
    }
    }


