using UnityEngine;
using System.IO;
using UnityEditor.Overlays;

public class SaveManager
{
    private string SavePath;

    void Awake()
    {
        SavePath = Path.Combine(Application.persistentDataPath, "SaveFile.Json");

    }

    public void SaveGame(ChatManager chatManager,BGMManager bgmManager) // 세이브 버튼에 연결할 onclick함수

    {
        SaveData Data = new SaveData();

    }
}
