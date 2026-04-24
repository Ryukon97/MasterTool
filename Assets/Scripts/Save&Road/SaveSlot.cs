using System.IO;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Slot : MonoBehaviour // 슬롯 구현을 위해 있는것 
{
    [Header("슬롯 세팅")]
    public int slotIndex;
    public TextMeshProUGUI infoText; // save버튼안에있는 날짜나 요약표시용 
    private SaveManager saveManager;
    private string savePath;
    void Start()
    {
        saveManager = FindAnyObjectByType<SaveManager>(); // 차후 덮어쓰기 형식같은 느낌의 로직구현하면 좋을듯?
        savePath = Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        string path = Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");

        if (File.Exists(path)) // 글씨안내판 코드상에서 뜨게 하는거 나중에 필요시 추가 
        {

            infoText.text = $"{slotIndex}번 데이터 존재함"; // 슬롯에 데이터 존재시 안내
            //infoText.color = Color.white;
        }
        else
        {
            // 파일이 없다면?
            infoText.text = "비어있음";
            infoText.color = new Color(1f, 1f, 1f, 0.5f); //비어있으면 약~간 투명
        }
    }

    public void OnClickSave()
    {
        saveManager.SaveGame(slotIndex);
        UpdateSlotUI();
    }

    public void OnclickLoad()
    {
        if (File.Exists(savePath))
        {
            saveManager.LoadGame(slotIndex);
        }
        else
        {
            Debug.Log($"{slotIndex}번 슬롯이 비어있어 불러올 수 없습니다");
        }
    
        //void Update()
        //{

        //}
    }
    public void OnclickSave()
    {
        if (File.Exists(savePath))
        {
            Debug.Log($"{slotIndex}");
        }
        saveManager.SaveGame(slotIndex);
        UpdateSlotUI();
    }
    public void OnclickDelet()
    {
        saveManager.DeletGame(slotIndex);
        UpdateSlotUI();
    }
}
