using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 구글시트에서 데이터를 불러와 Addressable로 로드된 SO에 반영하는 매니저 
/// 게임시작시 시트 데이터를 불러와 SO값을 덮어쓰기 (실시간 밸런스 패치용)
/// </summary>
public class GoogleLoader : Singleton<GoogleLoader>, IInitializable
{
    public async Task Init()
    {
        Debug.Log("GoogleLoader 데이터 로드 및 패치 시작.");
        int totalUpdated = 0;

        foreach(var kvp in URLContainer.SheetMap)
        {
            Type soType = kvp.Key;
            
            var (dataType, url) = kvp.Value;

            try
            {

                int updated = await LoadAndApply(soType, dataType, url);
                totalUpdated += updated; //이부분은 추후 삭제할듯

            }
            catch (Exception ex)
            {
                Debug.LogError($"GoogleLoader : {soType.Name} 패치중 오류 {ex.Message}");
            }
        }

        Debug.Log($"GoogleLoader : 전체 SO 패치 완료 총 {totalUpdated}개 갱신");
    }

    private async Task<int> LoadAndApply(Type soType, Type dataType, string url)
    {
        var method = typeof(GoogleSheetLoader).GetMethod("LoadSheetData")?.MakeGenericMethod(dataType);
        var task = method?.Invoke(null, new object[] { url }) as Task;
        await task.ConfigureAwait(false);

        var resultProp = task?.GetType().GetProperty("Result");
        var dataList = resultProp?.GetValue(task) as System.Collections.IList;

        if(dataList == null || dataList.Count == 0)
        {
            Debug.LogWarning($"GoogleLoader : {soType.Name} 시트 데이터가 비었거나 로드 실패");
            return 0;
        }

        return UpdateSOData(soType, dataType, dataList);
    }
    private int UpdateSOData(Type soType, Type dataType, System.Collections.IList dataList)
    {
        int updatedCount = 0;

        foreach(var kvp in AddressableLoader.Instance.loadedData)
        {
            foreach(var so in kvp.Value)
            {
                if (so.GetType() != soType || so is not BaseSO baseSO) continue;

                foreach(var data in dataList)
                {
                    var idField = dataType.GetField("ID");
                    var nameField = dataType.GetField("Name");

                    string dataID = idField?.GetValue(data) as string;
                    string dataName = nameField?.GetValue(data) as string;

                    if(dataID == baseSO.ID && dataName == baseSO.Name)
                    {
                        baseSO.ApplyData(data);
                        updatedCount++;
                        break;
                    }
                }
            }
        }

        Debug.Log($"GoogleLoader : {soType.Name} SO {updatedCount}개 갱신 완료");
        return updatedCount;
    }
}

