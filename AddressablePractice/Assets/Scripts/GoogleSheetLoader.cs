using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;

[System.Serializable]
public class CardSheetData //테스트용 임시
{
    public string ID;
    public string cardName;
    public int damage;
}

[System.Serializable]
public class CardSheetDataList
{
    public List<CardSheetData> list;
}
public static class GoogleSheetLoader
{
    public static async Task<List<CardSheetData>> LoadCardData(string url)
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
        CardSheetDataList wrapper = JsonUtility.FromJson<CardSheetDataList>(wrappedJson);

        Debug.Log($"GoogleSheetLoader : 데이터 로드 완료 {wrapper.list.Count}개");
        return wrapper.list;
    }
}
