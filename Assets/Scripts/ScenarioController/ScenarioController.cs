using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    public DialogueDataSO currentScenario;
    public int currentGroupIndex =0;
    public int currentEntryIndex =0;
   

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
