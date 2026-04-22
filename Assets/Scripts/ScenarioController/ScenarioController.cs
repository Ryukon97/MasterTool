using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    public DialogueDataSO currentScenario;
    public int currentGroupIndex =0;
    public int currentEntryIndex =0;
    //[Header("현재 진행중인 시나리오 데이터 (기/승/전/결 부분)")]
    //public DialogueDataSO currentScenario;
    //private ChatManager chatManager;
    //void Start()
    //{
    //    chatManager = FindAnyObjectByType<ChatManager >();

    //    if(currentScenario !=null)
    //    {
    //        StartChapter(currentScenario);
    //    }
    //}

    void Start()
    {
        if(currentScenario !=null)
        {
            StartChapter(currentScenario);
        }
    }
    public void StartChapter(DialogueDataSO newSO)
    {
        currentScenario = newSO;
        currentGroupIndex = 0;  
        currentEntryIndex = 0;   

        Debug.Log($"<color=pink>{newSO.name} 파트를 시작합니다!</color>");

        
        ChatManager chatManager = Object.FindAnyObjectByType<ChatManager>();
        if (chatManager != null && newSO.groups.Count > 0)
        {
            int firstID = newSO.groups[0].entries[0].id;
            chatManager.StartCoroutine(chatManager.PlayDialogue(firstID));
        }
    }

    //public void EndOfDialogue()
    //{
    //    if(currentScenario.nextStorySO != null)
    //    {
    //        Debug.Log("다음 시나리오로 진행됩니다");
    //        LoadChapter(currentScenario.nextStorySO);
    //    }
    //    else
    //    {
    //        FinishGame();
    //    }
    //}
    //void LoadChapter(DialogueDataSO nextSO)
    //{
    //    StartChapter(nextSO);
    //}
    //void FinishGame()
    //{
    //    Debug.Log("엔딩크레딧을 재생합니다 ");
    //}

    //void Update()
    //{

    //}


  

    public void RequestNextDialogue()
    {
        
        if (currentScenario.groups.Count == 0) return;

       
        DialogueGroup targetGroup = currentScenario.groups[currentGroupIndex];

      
        if (currentEntryIndex < targetGroup.entries.Count)
        {
            DialogueEntry data = targetGroup.entries[currentEntryIndex];

          
            currentEntryIndex++;
        }
        else
        {
          
            OnGroupFinished();
        }
    }

    public void EndOfDialogue()
    {
        if(currentScenario.nextStorySO !=null)
        {
            StartChapter(currentScenario.nextStorySO);
        }

    }

    void OnGroupFinished()
    {
        Debug.Log($"{currentScenario.groups[currentGroupIndex].GroupName} 그룹의 대사가 끝났습니다.");
      
    }
}
