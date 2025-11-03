using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 구글시트에서 데이터를 불러와 Addressable로 로드된 SO에 반영하는 매니저 
/// 게임시작시 시트 데이터를 불러와 SO값을 덮어쓰기 (실시간 밸런스 패치용)
/// </summary>
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

        //AddressableLoader에서 로드된 데이터 순회하면서 같은 ID랑 Name의 데이터를 보고 그값을 덮어씀
        foreach (var kvp in AddressableLoader.Instance.loadedData)
        {
            string label = kvp.Key;
            IList<ScriptableObject> soList = kvp.Value;
            int updatedCount = 0;

            //해당 라벨 SO 확인
            foreach (var so in soList)
            {
                if (so is not BaseSO baseSO) continue;

                CardSheetData match = dataList.Find(x => x.ID == baseSO.ID || x.cardName == baseSO.Name);

                if(match != null)
                {
                    baseSO.ApplyData(match);
                    updatedCount++;
                }
            }

            Debug.Log($"[GoogleLoader] [{label}] SO {updatedCount}개 갱신 완료");
            totalUpdated += updatedCount;
        }

        Debug.Log($"[GoogleLoader] 전체 라벨 SO 패치 완료 ({totalUpdated}개)");
    }
}

