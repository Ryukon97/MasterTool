using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SoundManagerEditor : EditorWindow
{
    private SoundDataSO SoundData;

    [MenuItem("MasterTools/Background Music Manager")]
    public static void ShowWindow()
    {
        GetWindow<SoundManagerEditor>("BGM Manager");
    }
    private void OnGUI()
    {
        GUILayout.Label("배경음 ID 관리자", EditorStyles.boldLabel);

        EditorGUI.EndChangeCheck();
        SoundData = (SoundDataSO)EditorGUILayout.ObjectField("Sound Data SO", SoundData, typeof(SoundDataSO), false);
        if(EditorGUI.EndChangeCheck())
        {
            Repaint();
        }
        if (SoundData == null)
        {
            EditorGUILayout.HelpBox("SoundDataSO 파일을 드래그해서 넣어주세요", MessageType.Warning);
            return;
        }
        EditorGUILayout.Space(10);
        if (GUILayout.Button("새 BGM이벤트 추가"))
        {
            if (SoundData.BGMEvents == null)
                SoundData.BGMEvents = new List<BGMEvent>();

            SoundData.BGMEvents.Add(new BGMEvent());

            EditorUtility.SetDirty(SoundData);
        }

        EditorGUILayout.Space(5);
        for (int i = 0; i < SoundData.BGMEvents.Count; i++)
        {
            var e = SoundData.BGMEvents[i];
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();
            e.EventName = EditorGUILayout.TextField("이름", e.EventName);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                SoundData.BGMEvents.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ID범위", GUILayout.Width(50));
            e.StartID = EditorGUILayout.IntField(e.StartID);
            EditorGUILayout.LabelField("~", GUILayout.Width(15));
            e.EndID = EditorGUILayout.IntField(e.EndID);
            EditorGUILayout.EndHorizontal();

            e.BGMIndex = EditorGUILayout.IntField("BGM인덱스", e.BGMIndex);
            e.FadeDuration = EditorGUILayout.Slider("디졸브 시간", e.FadeDuration, 0f, 5f);

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(SoundData);
            AssetDatabase.SaveAssets();
        }
        if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
        {
            Repaint();
        }
    }
}
