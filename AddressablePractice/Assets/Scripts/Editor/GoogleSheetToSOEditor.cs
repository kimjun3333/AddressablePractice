#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class GoogleSheetToSOEditor : EditorWindow
{
    private string url = "https://opensheet.elk.sh/1A-gVODBB_1QuNV0ko6rmo7rmnOWINJLhM_hyKEv-xks/Practice";

    [MenuItem("Tools/Google Sheet => SO 생성기")]
    public static void OpenWindow()
    {
        GetWindow<GoogleSheetToSOEditor>("Google Sheet Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Google Sheet URL", EditorStyles.boldLabel);
        url = EditorGUILayout.TextField("Sheet URL", url);

        if(GUILayout.Button("불러와서 SO생성 / 갱신"))
        {
            _ = ImportFromSheet();
        }
    }

    private async Task ImportFromSheet()
    {
        List<CardSheetData> dataList = await GoogleSheetLoader.LoadCardData(url);

        if(dataList == null)
        {
            Debug.LogError("GoogleSheetToSOEditor : 데이터 불러오기 실패");
            return;
        }

        SOGenerator.CreateOrUpdateCardSOs(dataList);
    }
}
#endif
