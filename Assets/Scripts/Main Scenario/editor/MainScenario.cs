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

        // 1. 파일 선택 필드
        currentSO = (DialogueDataSO)EditorGUILayout.ObjectField("편집할 파일", currentSO, typeof(DialogueDataSO), false);

        // 2. 파일이 없을 때의 처리 (중요!)
        if (currentSO == null)
        {
            EditorGUILayout.HelpBox("편집할 다이얼로그 데이터 SO를 드래그하거나 새로 생성하세요.", MessageType.Info);
            if (GUILayout.Button("새 시나리오 파일 생성"))
            {
                CreateNewSO();
            }
            return; // 마스타! 파일이 없으면 여기서 '진짜로' 멈춰야 아래 에러들이 안 납니다!
        }

        EditorGUILayout.Space();

        // 3. 스크롤 시작
        scrollpos = EditorGUILayout.BeginScrollView(scrollpos);

        try
        {
            // 4. 데이터 표시 로직 (안전하게 시리얼라이즈)
            SerializedObject serializedObject = new SerializedObject(currentSO);
            serializedObject.Update(); // 최신 데이터로 업데이트

            SerializedProperty entriesProperty = serializedObject.FindProperty("entries");
            if (entriesProperty != null)
            {
                EditorGUILayout.PropertyField(entriesProperty, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
        catch (System.Exception e)
        {
            // 혹시라도 에러가 나면 여기서 잡아줌
            Debug.LogWarning("데이터를 표시하는 중 오류 발생: " + e.Message);
        }

        // 5. 스크롤 종료 (Begin과 짝을 맞춤)
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        // 6. 저장 버튼
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

        // 1. 폴더 경로 (마스타가 지정한 경로)
        string folderPath = "Assets/Scripts/Main Scenario/ScenarioGallery";

        // 2. 파일 이름 설정
        string fileName = "NewScenario.asset";

        // 3. 전체 경로 조합 (폴더 + 파일이름)
        string fullPath = folderPath + "/" + fileName;

        // 4. 폴더가 실제로 있는지 체크 (안전장치)
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogWarning($"<color=orange>{folderPath} 폴더가 없어서 Assets 루트에 생성합니다.</color>");
            fullPath = "Assets/" + fileName;
        }

        // 5. [중요] 중복된 이름이 있다면 NewScenario 1.asset 식으로 이름을 바꿔줌
        fullPath = AssetDatabase.GenerateUniqueAssetPath(fullPath);

        // 6. [핵심수정] 반드시 'fullPath'를 넣어야 합니다!
        AssetDatabase.CreateAsset(asset, fullPath);

        AssetDatabase.SaveAssets();
        currentSO = asset;

        // 생성된 파일을 프로젝트 창에서 강조 표시
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        Debug.Log($"<color=green>새 시나리오 생성 완료: {fullPath}</color>");
    }
}
