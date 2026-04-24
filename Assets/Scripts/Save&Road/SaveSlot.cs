using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour // 슬롯 구현을 위해 있는것 
{
    public int slotIndex;
    public TextMeshProUGUI infoText; // save버튼안에있는 날짜나 요약표시용 

    private SaveManager saveManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveManager = FindAnyObjectByType<SaveManager>(); // 차후 덮어쓰기 형식같은 느낌의 로직구현하면 좋을듯?
    }

    public void OnClickSave()
    {
        saveManager.SaveGame(slotIndex);
    }

    public void OnclickLoad()
    {
        saveManager.LoadGame(slotIndex);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
