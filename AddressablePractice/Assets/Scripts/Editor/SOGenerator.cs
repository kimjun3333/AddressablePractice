#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// GoogleSheetToSOEditor과 세트인 SO 생성/갱신 유틸리티 클래스
/// </summary>
public static class SOGenerator
{
    private const string baseFolder = "Assets/SO"; //SO 파일 저장 경로
    private static readonly Dictionary<(Type, string), FieldInfo> fieldCache = new(); //캐시 추가

    public static void CreateOrUpdateSOs<TSO, TData>(List<TData> dataList) where TSO : BaseSO
    {
        if (!AssetDatabase.IsValidFolder(baseFolder))
        {
            AssetDatabase.CreateFolder("Assets", "SO");
        }

        string typeFolder = $"{baseFolder}/{typeof(TSO).Name}";
        if (!AssetDatabase.IsValidFolder(typeFolder))
            AssetDatabase.CreateFolder(baseFolder, typeof(TSO).Name);

        //디버그용 지역 변수
        int created = 0;
        int updated = 0;

        foreach (var data in dataList)
        {
            string id = GetFieldValue<string>(data, "ID");
            string name = GetFieldValue<string>(data, "Name");
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(id)) continue;

            string safeName = Regex.Replace(name, @"[\/\\:\*\?""<>\| ]", "_");
            string assetPath = $"{typeFolder}/{id}_{safeName}.asset";
            TSO so = AssetDatabase.LoadAssetAtPath<TSO>(assetPath);

            if(so == null)
            {
                string[] guids = AssetDatabase.FindAssets($"{id}_ t:{typeof(TSO).Name}", new[] { typeFolder });
                foreach(var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    TSO candidate = AssetDatabase.LoadAssetAtPath<TSO>(path);
                    if(candidate != null && candidate.ID == id)
                    {
                        if(path != assetPath)
                        {
                            AssetDatabase.RenameAsset(path, $"{id}_{safeName}");
                            Debug.Log($"SOGenerator : {typeof(TSO).Name} 이름 변경됨 => {path} → {assetPath}");
                        }

                        so = candidate;
                        break;
                    }
                }
            }

            if (so == null)
            {
                so = ScriptableObject.CreateInstance<TSO>();
                so.ID = id;
                so.Name = name;
                AssetDatabase.CreateAsset(so, assetPath);
                created++;
            }
            else
            {
                if(so.Name != name)
                {
                    Debug.Log($"SOGenerator : {typeof(TSO).Name}: ID({id})는 동일하지만 이름 변경됨 => {so.Name} => {name}");
                    so.Name = name;
                }

                updated++;
            }

            so.ApplyData(data);
            EditorUtility.SetDirty(so);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"SOGenerator : {typeof(TSO).Name} 생성 {created}개, 갱신 {updated}개 완료");
    }

    /// <summary>
    /// 리플렉션 + 캐시 기반 필드 접근
    /// </summary>
    private static T GetFieldValue<T>(object obj, string fieldName)
    {
        if (obj == null) return default;

        var type = obj.GetType();
        var key = (type, fieldName);

        if(!fieldCache.TryGetValue(key, out var field))
        {
            field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            fieldCache[key] = field;
        }

        if (field != null && field.FieldType == typeof(T)) 
            return (T)field.GetValue(obj);

        return default;
    }

    /// <summary>
    /// 시트에 존재하지 않는 SO 삭제(자동 정리용)
    /// </summary>
    /// <typeparam name="TSO"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <param name="dataList"></param>
    public static void CleanUpOrphanedSOs<TSO, TData>(List<TData> dataList) where TSO : BaseSO //GoogleSheetToSOEditor에서 사용중
    {
        string typeFolder = $"{baseFolder}/{typeof(TSO).Name}";
        if (!AssetDatabase.IsValidFolder(typeFolder)) return;

        HashSet<string> validIds = new();
        foreach(var data in dataList)
        {
            string id = GetFieldValue<string>(data, "ID");
            if(!string.IsNullOrEmpty(id))
            {
                validIds.Add(id);
            }
        }

        string[] guids = AssetDatabase.FindAssets($"t:{typeof(TSO).Name}", new[] { typeFolder });
        int deleted = 0;

        foreach(var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TSO so = AssetDatabase.LoadAssetAtPath<TSO>(path);
            if(so != null && !validIds.Contains(so.ID))
            {
                AssetDatabase.DeleteAsset(path);
                deleted++;
                Debug.Log($"SOGenerator : {typeof(TSO).Name} '{so.ID}' 제거됨 (시트에 없음)");
            }
        }

        if(deleted > 0)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"<color=red>[SOGenerator]</color> {typeof(TSO).Name} {deleted}개 삭제 완료 (시트에 없음)");
        }
        else
        {
            Debug.Log($"<color=gray>[SOGenerator]</color> {typeof(TSO).Name} 불필요한 SO 없음 (깨끗함)");
        }
    }

    /// <summary>
    /// 캐시 초기화 (시트 구조 변경시 호출됨) 
    /// </summary>
    public static void ClearFieldCache()
    {
        fieldCache.Clear();
        Debug.Log("<color=yellow>[SOGenerator]</color> Reflection Field 캐시 초기화 완료");
    }
}
#endif