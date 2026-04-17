using UnityEngine;
using UnityEditor;
using UnityEngine.WSA;
using System.Collections.Generic;

public class MainScenario : EditorWindow
{
    private DialogueDataSO currentSO;
    private SerializedObject so;
    private SerializedProperty entriesProperty;
    private Vector2 scrollpos;

    [MenuItem("MasterTools/Scenario Editor")]
    public static void ShowWindow()
    {
        GetWindow<MainScenario>("시나리오 에디터");

    }
    void OnGUI()
    {

        GUILayout.Label("시나리오 편집 모드", EditorStyles.boldLabel);
        currentSO = (DialogueDataSO)EditorGUILayout.ObjectField("편집할 파일", currentSO, typeof(DialogueDataSO), false);


        if (currentSO == null)
        {
            EditorGUILayout.HelpBox("편집할 다이얼로그 데이터 SO를 드래그하거나 새로 생성하세요.", MessageType.Info);
            if (GUILayout.Button("새 시나리오 파일 생성")) CreateNewSO();
            return;
        }

        EditorGUILayout.Space();
        scrollpos = EditorGUILayout.BeginScrollView(scrollpos);

        SerializedObject serializedObject = new SerializedObject(currentSO);
        serializedObject.Update();

        SerializedProperty entriesProperty = serializedObject.FindProperty("entries");
      
        if (entriesProperty != null)
        {
          
            entriesProperty.isExpanded = EditorGUILayout.Foldout(entriesProperty.isExpanded, "Entries", true);

            if (entriesProperty.isExpanded)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                {
                    int currentSize = entriesProperty.arraySize;
                    int Newsize = EditorGUILayout.IntField("Size", currentSize);

                    if (GUILayout.Button("+", GUILayout.Width(30))) Newsize++;
                    if (GUILayout.Button("-", GUILayout.Width(30)) && Newsize > 0) Newsize--;
                    if(Newsize != currentSize) entriesProperty.arraySize = Newsize;

                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(5);
              
                //int newSize = EditorGUILayout.IntField("Size", entriesProperty.arraySize);
                //if (newSize != entriesProperty.arraySize) entriesProperty.arraySize = newSize;

                for (int i = 0; i < entriesProperty.arraySize; i++)
                {
                    SerializedProperty element = entriesProperty.GetArrayElementAtIndex(i);

                   
                    string sName = element.FindPropertyRelative("speakerName").stringValue;
                    string label = $"[ID: {i}] " + (string.IsNullOrEmpty(sName) ? "이름 없음" : sName);

                  
                    element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, label, true);

                    if (element.isExpanded)
                    {
                        EditorGUI.indentLevel++; 

                        
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("speakerName"));
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("dialogueText"));
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("characterIllust"));
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("choices"), true);
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("nextIndexOverride"));

                        EditorGUI.indentLevel--; 
                        EditorGUILayout.Space(2);
                    }
                }
                EditorGUI.indentLevel--; // 부모 종료
            }
        }

        serializedObject.ApplyModifiedProperties();
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
