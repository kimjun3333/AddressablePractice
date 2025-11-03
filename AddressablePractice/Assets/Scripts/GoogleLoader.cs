using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class GoogleLoader : Singleton<GoogleLoader>, IInitializable
{
    private string url = "https://opensheet.elk.sh/1A-gVODBB_1QuNV0ko6rmo7rmnOWINJLhM_hyKEv-xks/Practice";
    public async Task Init()
    {
        Debug.Log("GoogleLoader 데이터 로드 및 패치 시작.");

        List<CardSheetData> dataList = await GoogleSheetLoader.LoadCardData(url);
        if (dataList == null || dataList.Count == 0)
        {
            Debug.LogError($"GoogleLoader : sheet 데이터를 불러오지 못했습니다.");
            return;
        }

        int totalUpdated = 0;

        foreach (var kvp in AddressableLoader.Instance.loadedData)
        {
            string label = kvp.Key;
            IList<ScriptableObject> soList = kvp.Value;
            int updatedCount = 0;

            foreach (var so in soList)
            {
                if (so is CardSO card)
                {
                    CardSheetData match = dataList.Find(x =>
                        x.ID == card.name || x.cardName == card.cardName);

                    if (match != null)
                    {
                        card.cardName = match.cardName;
                        card.damage = match.damage;
                        updatedCount++;
                    }
                }
            }

            Debug.Log($"[GoogleLoader] [{label}] SO {updatedCount}개 갱신 완료");
            totalUpdated += updatedCount;
        }

        Debug.Log($"[GoogleLoader] 전체 라벨 SO 패치 완료 ({totalUpdated}개)");
    }


}

