using System.IO;
using TMPro;
#if UNITY_EDITOR
using UnityEditor.Overlays;
#endif
using UnityEngine;

public class Slot : MonoBehaviour // 슬롯 구현을 위해 있는것 
{

    [Header("슬롯 세팅")]
    public int slotIndex = 0;
    public TextMeshProUGUI infoText; // save버튼안에있는 날짜나 요약표시용 
    private SaveManager saveManager;
    private string savePath;

    public GameObject DeleteConfirmPanel; //게임 삭제 
    private static int PendingDeleteIndex;

    [Header("UI 패널 연결")]
    public GameObject DeleteSlotButtonGroup;
    public GameObject GameSlotAll;

    [Header("안내 문구 설정")]
    public GameObject NoDataWarningUI;

    void Start()
    {
        saveManager = FindAnyObjectByType<SaveManager>(); // 차후 덮어쓰기 형식같은 느낌의 로직구현하면 좋을듯?
        //savePath = Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {

        this.savePath = Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");
        bool fileExists = File.Exists(savePath);

        if (infoText != null)
        {
            if (fileExists)
            {

                string json = File.ReadAllText(savePath);


                SaveAndLoad data = JsonUtility.FromJson<SaveAndLoad>(json);

                string Sodataname = string.IsNullOrEmpty(data.SOdataName) ? "Unknown Scene" : data.SOdataName;
                string displayTime = string.IsNullOrEmpty(data.SaveToday) ? "시간 정보 없음" : data.SaveToday;


                infoText.text = $"<color=black>{Sodataname}</color>\n<size=80%>{displayTime}</size>";
                infoText.color = Color.white;
            }
            else
            {
                infoText.text = "비어있음";
                infoText.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }

        else
        {
            Debug.LogError($"[Slot UI] {gameObject.name}에 InfoText가 연결되지 않았습니다!");
        }
    }




    public void OnClickSave()
    {
        saveManager.SaveGame(slotIndex);
        UpdateSlotUI();
        Debug.Log($"<color=cyan>{slotIndex}번 슬롯에 저장 완료!</color>");
    }
    //public void OnclickSave()
    //{
    //    if (File.Exists(savePath))
    //    {
    //        Debug.Log($"{slotIndex}");
    //    }
    //    saveManager.SaveGame(slotIndex);
    //    UpdateSlotUI();
    //}
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


    }
    public void OnclickDelete()
    {
        saveManager.DeletGame(slotIndex);
        UpdateSlotUI();
    }

    public void OnclickDeleteRequest()
    {
        savePath = Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");

        if (File.Exists(savePath))
        {
            if (DeleteConfirmPanel != null)
            {
                DeleteConfirmPanel.SetActive(true);
                if (NoDataWarningUI != null) NoDataWarningUI.SetActive(false);
            }

            //PendingDeleteIndex = slotIndex;
            //if (DeleteConfirmPanel != null)
            //{
            //    if (DeleteSlotButtonGroup != null) DeleteSlotButtonGroup.SetActive(false);
            //DeleteConfirmPanel.SetActive(true);
            //}
        }
        else
        {

            //if (NoDataWarningUI != null)
            //{

            //    StopAllCoroutines();
            //    StartCoroutine(ShowNoDataNotice());
            //}

            Debug.Log("삭제할 데이터가 없습니다");
            if (NoDataWarningUI != null)
            {
                StopAllCoroutines();
                StartCoroutine(ShowNoDataNotice());
            }
        }
    }

    private void OnEnable()
    {
        UpdateSlotUI();
    }

    private System.Collections.IEnumerator ShowNoDataNotice()
    {

        NoDataWarningUI.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        NoDataWarningUI.SetActive(false);
    }

    public void ConfirmDelete() // 삭제용 함수 이거 넣으면 모든버튼이 삭제버튼화됨 ''' 주의 '''
    {

        Debug.Log($"<color=cyan>[확인] {gameObject.name} 슬롯에서 삭제 실행!</color>");


        if (saveManager != null)
        {
            saveManager.DeletGame(slotIndex);
        }
        else
        {
            Debug.LogError("SaveManager가 연결되지 않았습니다!");
            saveManager = FindAnyObjectByType<SaveManager>();
            saveManager.DeletGame(slotIndex);
        }


        UpdateSlotUI();


        if (DeleteConfirmPanel != null)
        {
            DeleteConfirmPanel.SetActive(false);
        }
    }
    public void CancelDelete() // 아니요 버튼에 연결할 함수
    {
        Debug.Log("<color=white>[Slot UI] 삭제 취소됨 - 패널 닫기</color>");

        if (DeleteConfirmPanel != null)
        {

            DeleteConfirmPanel.SetActive(false);
        }
    }

}
