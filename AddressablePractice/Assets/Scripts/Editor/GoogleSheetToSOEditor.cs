#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Reflection;

/// <summary>
/// Google스프레드 시트 데이터 기반으로 SO를 생성, 갱신해주는 커스텀 툴 
/// </summary>
public class GoogleSheetToSOEditor : EditorWindow
{
    /// <summary>
    /// Unity메뉴바에 표시되는 메뉴 항목 등록
    /// </summary>
    [MenuItem("Tools/Google Sheet => SO 생성기")]
    public static void OpenWindow()
    {
        GetWindow<GoogleSheetToSOEditor>("Google Sheet Importer");
    }

    /// <summary>
    /// 에디터창 GUI 그리기
    /// </summary>
    private void OnGUI()
    {
        GUILayout.Label("Google Sheet URL", EditorStyles.boldLabel);
        GUILayout.Space(10);

        //버튼 클릭시 ImportFromSheet() 비동기 실행
        
        foreach(var kvp in URLContainer.SheetMap)
        {
            Type soType = kvp.Key;
            var (dataType, url) = kvp.Value;

            GUILayout.Label($"{soType.Name} : {url}", EditorStyles.miniLabel);

            if(GUILayout.Button($"{soType.Name} SO 생성 / 갱신"))
            {
                _ = ImportData(soType, dataType, url);
            }
        }

        GUILayout.Space(10);
        if(GUILayout.Button("모든 시트SO 생성 / 갱신"))
        {
            _ = ImportAllData();
        }
       
    }

    /// <summary>
    /// 데이터를 비동기로 불러오고 데이터 기반으로 SO를 생성, 갱신
    /// </summary>
    /// <returns></returns>
    
    private async Task ImportData(Type soType, Type dataType, string url)
    {
        var method = typeof(GoogleSheetLoader).GetMethod("LoadSheetData")?.MakeGenericMethod(dataType);

        if (method == null)
        {
            Debug.LogError($"[ERROR] {soType.Name} : GoogleSheetLoader 메서드 못찾음");
            return;
        }

        var task = method?.Invoke(null, new object[] { url }) as Task;
        //await task.ConfigureAwait(false);
        await task;

        var resultProp = task?.GetType().GetProperty("Result");
        var dataList = resultProp?.GetValue(task) as System.Collections.IList;

        if(dataList == null)
        {
            Debug.LogError($"GoogleSheetToSOEditor : {soType.Name} 시트 데이터 불러오기 실패");
            return;
        }

        var generatorMethod = typeof(SOGenerator).GetMethod("CreateOrUpdateSOs", BindingFlags.Public | BindingFlags.Static)?.MakeGenericMethod(soType, dataType);

        if (generatorMethod == null)
        {
            Debug.LogError($"[ERROR] {soType.Name} : SOGenerator 메서드 찾기 실패 ❌");
            return;
        }

        generatorMethod?.Invoke(null, new object[] { dataList });

        Debug.Log($"GoogleSheetToSOEditor : {soType.Name} SO {dataList.Count}개 생성/갱신 완료");
    }

    private async Task ImportAllData()
    {
        foreach(var kvp in URLContainer.SheetMap)
        {
            await ImportData(kvp.Key, kvp.Value.dataType, kvp.Value.url);
        }

        Debug.Log("GoogleSheetToSOEditor : 모든 시트 SO 생성 / 갱신 완료");
    }
}
#endif
