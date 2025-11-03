#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Google스프레드 시트 데이터 기반으로 SO를 생성, 갱신해주는 커스텀 툴 
/// </summary>
public class GoogleSheetToSOEditor : EditorWindow
{
    private string url = "https://opensheet.elk.sh/1A-gVODBB_1QuNV0ko6rmo7rmnOWINJLhM_hyKEv-xks/Card"; //URL 주소

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
        url = EditorGUILayout.TextField("Sheet URL", url);

        //버튼 클릭시 ImportFromSheet() 비동기 실행
        if(GUILayout.Button("불러와서 SO생성 / 갱신"))
        {
            _ = ImportCardData();
        }
    }

    /// <summary>
    /// 데이터를 비동기로 불러오고 데이터 기반으로 SO를 생성, 갱신
    /// </summary>
    /// <returns></returns>
    private async Task ImportCardData()
    {
        List<CardSheetData> dataList = await GoogleSheetLoader.LoadCardData(url);

        if(dataList == null)
        {
            Debug.LogError("GoogleSheetToSOEditor : 데이터 불러오기 실패");
            return;
        }

        SOGenerator.CreateOrUpdateSOs<CardSO, CardSheetData>(dataList);
    }
}
#endif
