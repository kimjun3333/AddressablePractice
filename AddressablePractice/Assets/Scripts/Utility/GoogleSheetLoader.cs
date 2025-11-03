using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System;

/// <summary>
/// 구글시트 Json을 JsonUtility로 파싱하기 위한 래퍼 클래스
/// </summary>

[Serializable]
public class SheetDataList<T>
{
    public List<T> list;
}

/// <summary>
/// 구글시트로 부터 데이터를 받아와 List<CardSheetData>로 반환하는 유틸리티 클래스.
/// </summary>
public static class GoogleSheetLoader
{
    public static async Task<List<T>> LoadSheetData<T>(string url)
    {
        using UnityWebRequest req = UnityWebRequest.Get(url);
        var op = req.SendWebRequest();

        while(!op.isDone)
        {
            await Task.Yield();
        }

        if(req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"GoogleSheetLoader : 요청 실패: {req.error}");
            return null;
        }

        string rawJson = req.downloadHandler.text;

        string wrappedJson = "{\"list\":" + rawJson + "}";
        SheetDataList<T> wrapper = JsonUtility.FromJson<SheetDataList<T>>(wrappedJson);

        Debug.Log($"GoogleSheetLoader : 데이터 로드 완료 {wrapper.list.Count}개");
        return wrapper.list;
    }
}
