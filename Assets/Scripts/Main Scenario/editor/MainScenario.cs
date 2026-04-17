using UnityEngine;
using UnityEditor;
using UnityEngine.WSA;

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
        GUILayout.Label("시나리오 편집 모드", EditorStyles.boldLabel);

        
        currentSO = (DialogueDataSO)EditorGUILayout.ObjectField("편집할 파일", currentSO, typeof(DialogueDataSO), false);

       
        if (currentSO == null)
        {
            EditorGUILayout.HelpBox("편집할 다이얼로그 데이터 SO를 드래그하거나 새로 생성하세요.", MessageType.Info);
            if (GUILayout.Button("새 시나리오 파일 생성"))
            {
                CreateNewSO();
            }
            return; 
        }

        EditorGUILayout.Space();

       
        scrollpos = EditorGUILayout.BeginScrollView(scrollpos);

        try
        {
            
            SerializedObject serializedObject = new SerializedObject(currentSO);
            serializedObject.Update(); // 최신 데이터로 업데이트

            SerializedProperty entriesProperty = serializedObject.FindProperty("entries");
            for (int i = 0; i < entriesProperty.arraySize; i++) // 선택지를위한 보기용 코드
            {
                SerializedProperty element = entriesProperty.GetArrayElementAtIndex(i);

             
                EditorGUILayout.BeginVertical("box"); 
                EditorGUILayout.LabelField($" [ID: {i}] 번 대사 구역", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(element, true);

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }
            if (entriesProperty != null)
            {
                EditorGUILayout.PropertyField(entriesProperty, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
        catch (System.Exception e)
        {
            
            Debug.LogWarning("데이터를 표시하는 중 오류 발생: " + e.Message);
        }

        
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        
        if (GUILayout.Button("저장(Force Save)", GUILayout.Height(30)))
        {
            EditorUtility.SetDirty(currentSO);
            AssetDatabase.SaveAssets();
            Debug.Log("<color=cyan>시나리오 데이터 저장 완료!</color>");
        }
    }
    private void CreateNewSO()
    {
        DialogueDataSO asset = ScriptableObject.CreateInstance<DialogueDataSO>();

       
        string folderPath = "Assets/Scripts/Main Scenario/ScenarioGallery";

        
        string fileName = "NewScenario.asset";

       
        string fullPath = folderPath + "/" + fileName;

        
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogWarning($"<color=orange>{folderPath} 폴더가 없어서 Assets 루트에 생성합니다.</color>");
            fullPath = "Assets/" + fileName;
        }

       
        fullPath = AssetDatabase.GenerateUniqueAssetPath(fullPath);

        
        AssetDatabase.CreateAsset(asset, fullPath);

        AssetDatabase.SaveAssets();
        currentSO = asset;

        
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        Debug.Log($"<color=green>새 시나리오 생성 완료: {fullPath}</color>");
    }
}
