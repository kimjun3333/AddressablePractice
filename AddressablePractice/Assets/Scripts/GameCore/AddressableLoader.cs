using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// Addressable 비동기 로드하는 매니저.
/// </summary>

public class AddressableLoader : Singleton<AddressableLoader>, IInitializable
{
    public Dictionary<string, IList<ScriptableObject>> loadedData = new();

    public async Task Init()
    {
        await Addressables.InitializeAsync().Task; //Addressable 시스템 초기화

        string[] labels = { "Attack", "Defense" }; //라벨목록 관리 이후 안정화 되면 라벨 자동 탐색후 추가하도록 변경할것

        foreach (var label in labels) //라벨별로 순회하면서 로드
        {
            var handle = Addressables.LoadAssetsAsync<ScriptableObject>(label, null);
            IList<ScriptableObject> assets = await handle.Task;
            loadedData[label] = assets; //딕셔너리에 등록
        }

        Debug.Log("Addressable SO 로드 완료");
    }
}