using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 구글시트에서 데이터를 불러와 Addressable로 로드된 SO에 반영하는 매니저 
/// 게임시작시 시트 데이터를 불러와 SO값을 덮어쓰기 (실시간 밸런스 패치용)
/// </summary>
public class GoogleLoader : Singleton<GoogleLoader>, IInitializable
{
    string cardURL = URLContainer.cardURL;
    string artifactURL = URLContainer.artifactURL;

    public async Task Init()
    {
        Debug.Log("GoogleLoader 데이터 로드 및 패치 시작.");

        int totalUpdated = 0;

        var cardData = await GoogleSheetLoader.LoadSheetData<CardSheetData>(cardURL);
        if(cardData != null && cardData.Count > 0)
        {
            int updated = UpdateSOData<CardSO, CardSheetData>(cardData);
            Debug.Log($"GoogleLoader : 카드 SO {updated}개 갱신 완료");
            totalUpdated += updated;
        }
        else
        {
            Debug.LogError("GoogleLoader : 카드 시트 데이터를 불러오지 못했습니다.");
        }

        var artifactData = await GoogleSheetLoader.LoadSheetData<ArtifactSheetData>(artifactURL);
        if(artifactData != null && artifactData.Count > 0)
        {
            int updated = UpdateSOData<ArtifactSO, ArtifactSheetData>(artifactData);
            Debug.Log($"GoogleLoader : 유물 SO {updated}개 갱신 완료");
            totalUpdated += updated;
        }
        else
        {
            Debug.LogError("GoogleLoader : 유물 시트 데이터를 불러오지 못했습니다.");
        }

        Debug.Log($"GoogleLoader : 전체 SO 패치 완료 총 {totalUpdated}개 갱신됨");
    }

    private int UpdateSOData<TSO, TData>(List<TData> dataList) where TSO : BaseSO where TData : BaseSheetData
    {
        int updatedCount = 0;

        foreach(var kvp in AddressableLoader.Instance.loadedData)
        {
            string label = kvp.Key;
            IList<ScriptableObject> soList = kvp.Value;

            foreach(var so in soList)
            {
                if (so is not BaseSO baseSO) continue;

                TData match = dataList.Find(x => x.ID == baseSO.ID || x.Name == baseSO.Name);
                if (match == null) continue;

                baseSO.ApplyData(match);
                updatedCount++;
            }
        }

        return updatedCount;
    }
}

