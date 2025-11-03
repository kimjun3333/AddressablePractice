#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// GoogleSheetToSOEditor과 세트인 SO 생성/갱신 유틸리티 클래스
/// </summary>
public static class SOGenerator
{
    private const string baseFolder = "Assets/SO"; //SO 파일 저장 경로

    public static void CreateOrUpdateSOs<TSO, TData>(List<TData> dataList) where TSO : BaseSO
    {
        if (!AssetDatabase.IsValidFolder(baseFolder))
        {
            AssetDatabase.CreateFolder("Assets", "SO");
        }

        string typeFolder = $"{baseFolder}/{typeof(TSO).Name}";
        if (!AssetDatabase.IsValidFolder(typeFolder))
            AssetDatabase.CreateFolder(baseFolder, typeof(TSO).Name);

        foreach (var data in dataList)
        {
            string id = GetFieldValue<string>(data, "ID");
            string name = GetFieldValue<string>(data, "Name");
            if (string.IsNullOrEmpty(name)) continue;

            string assetPath = $"{typeFolder}/{name}.asset";
            TSO so = AssetDatabase.LoadAssetAtPath<TSO>(assetPath);

            if (so == null)
            {
                so = ScriptableObject.CreateInstance<TSO>();
                so.Name = name;
                AssetDatabase.CreateAsset(so, assetPath);
            }

            so.ApplyData(data);
            EditorUtility.SetDirty(so);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[SOGenerator] {typeof(TSO).Name} {dataList.Count}개 생성/갱신 완료 ✅");
    }



    private static T GetFieldValue<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null && field.FieldType == typeof(T))
            return (T)field.GetValue(obj);
        return default;
    }
}
#endif