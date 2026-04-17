using UnityEngine;
using UnityEditor;

public class MainScenario : EditorWindow
{
    private DialogueDataSO currentSO;
    private Vector2 scrollpos;

    [MenuItem("MasterTools/Scenario Editor")]
    public static void ShowWindow()
        {
        GetWindow<MainScenario>("시나리오 에디터");

    }
    private void OnGUI()
    {
        GUILayout.Label("시나리오 편집 모드",EditorStyles.boldLabel);

        currentSO = (DialogueDataSO)EditorGUILayout.ObjectField("편집할 파일", currentSO,typeof(DialogueDataSO),false);
        
        if(currentSO == null)
        {
            EditorGUILayout.HelpBox("편집할 다이얼로그데이터 SO를 드래그하거나 새로생헝하세요", MessageType.Info);
            if( GUILayout.Button("새 시나리오 파일 생성")) { CreateNewSO(); }

        }
        EditorGUILayout.Space();
        scrollpos = GUILayout.BeginScrollView(scrollpos);

        SerializedObject SerializedObject = new SerializedObject(currentSO);
        SerializedProperty Entriessproperty = SerializedObject.FindProperty("entries");

        EditorGUILayout.PropertyField(Entriessproperty, true);

        SerializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("저장(Force Save)"))
        {
            EditorUtility.SetDirty(currentSO);
            AssetDatabase.SaveAssets();
            Debug.Log("시나리오 데이터 저장");
        }
    }
    private void CreateNewSO()
    {
        DialogueDataSO asset = ScriptableObject.CreateInstance<DialogueDataSO>();
        string path = "Assets/NewScenario.asset";
        AssetDatabase.CreateAsset(asset, path);
     
        AssetDatabase.SaveAssets();
        currentSO = asset;

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        Debug.Log("새 시나리오 파일 생성 완료!");
    }
}
