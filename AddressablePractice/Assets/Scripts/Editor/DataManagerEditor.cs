using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var dm = (DataManager)target;
        var field = typeof(DataManager)
            .GetField("dataByLabel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var dict = field.GetValue(dm) as Dictionary<string, List<ScriptableObject>>;
        if (dict == null) return;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("dataByLabel (런타임 상태)", EditorStyles.boldLabel);
        foreach(var kv  in dict)
        {
            EditorGUILayout.LabelField($"{kv.Key}: {kv.Value.Count}개");
            foreach (var so in kv.Value)
                EditorGUILayout.ObjectField(so, typeof(ScriptableObject), false);
        }
    }
}
