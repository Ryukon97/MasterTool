using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MainScenario : EditorWindow
{
    private DialogueDataSO currentSO;
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
            //EditorGUILayout.HelpBox("편집할 다이얼로그 데이터 SO를 드래그하거나 새로 생성하세요.", MessageType.Info);
            if (GUILayout.Button("새 시나리오 파일 생성")) CreateNewSO();
            return;
        }

        //EditorGUILayout.Space();
        //scrollpos = EditorGUILayout.BeginScrollView(scrollpos);

        //SerializedObject serializedObject = new SerializedObject(currentSO);
        //serializedObject.Update();
        SerializedObject serializedObject = new SerializedObject(currentSO);
        serializedObject.Update();

        scrollpos = EditorGUILayout.BeginScrollView(scrollpos);

      

        SerializedProperty entriesProperty = serializedObject.FindProperty("entries");

        if (entriesProperty != null)
        {
          
            EditorGUILayout.BeginHorizontal();
            {
                entriesProperty.isExpanded = EditorGUILayout.Foldout(entriesProperty.isExpanded, "전체 대사 리스트 (Entries)", true);
                GUILayout.FlexibleSpace();

                int currentSize = entriesProperty.arraySize;
                EditorGUILayout.LabelField("Size", GUILayout.Width(35));
                int newSize = EditorGUILayout.IntField(currentSize, GUILayout.Width(50));

                if (GUILayout.Button("+", GUILayout.Width(25))) newSize++;
                if (GUILayout.Button("-", GUILayout.Width(25)) && newSize > 0) newSize--;

                if (newSize != currentSize) entriesProperty.arraySize = newSize;
            }
            EditorGUILayout.EndHorizontal();

            if (entriesProperty.isExpanded)
            {
                EditorGUILayout.Space(5);
                EditorGUI.indentLevel++;

                for (int i = 0; i < entriesProperty.arraySize; i++)
                {


                    SerializedProperty element = entriesProperty.GetArrayElementAtIndex(i);
                    SerializedProperty idProp = element.FindPropertyRelative("id");
                    SerializedProperty nameProp = element.FindPropertyRelative("speakerName");

                    int displayID = (idProp != null) ? idProp.intValue : i;
                    string sName = (nameProp != null) ? nameProp.stringValue : "";
                    string label = $"[ID: {displayID}] " + (string.IsNullOrEmpty(sName) ? "이름 없음" : sName);

                    element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, label, true);

                    if (element.isExpanded)
                    {
                        EditorGUI.indentLevel++;

                        if (idProp != null) EditorGUILayout.PropertyField(idProp, new GUIContent("고유 ID"));

                        EditorGUILayout.PropertyField(nameProp);
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("dialogueText"));

                      
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("characterIllust"));
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("CharacterPNG"));
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("CharacterPos"));
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("MoveDuration"));
                        EditorGUILayout.Space(5);



                        SerializedProperty bgProp = element.FindPropertyRelative("BackGroundSprit");
                        if (bgProp != null)
                        {
                            EditorGUILayout.PropertyField(bgProp);
                        }

                        EditorGUILayout.PropertyField(element.FindPropertyRelative("choices"), true);
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("nextIndexOverride"));

                        EditorGUI.indentLevel--;
                    }
                }

            
                EditorGUILayout.EndScrollView();

                EditorGUILayout.Space();


                if(serializedObject.hasModifiedProperties)
                {
                    serializedObject.ApplyModifiedProperties();
                }
                if (GUILayout.Button("저장(Force Save)", GUILayout.Height(30)))
                {
                    EditorUtility.SetDirty(currentSO);
                    AssetDatabase.SaveAssets();
                    Debug.Log("<color=cyan>시나리오 데이터 저장 완료!</color>");
                }
            }
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
            fullPath = "Assets/" + fileName;
        }

        fullPath = AssetDatabase.GenerateUniqueAssetPath(fullPath);
        AssetDatabase.CreateAsset(asset, fullPath);
        AssetDatabase.SaveAssets();
        currentSO = asset;

        Selection.activeObject = asset;
        Debug.Log($"<color=green>새 시나리오 생성 완료: {fullPath}</color>");
    }

   

}