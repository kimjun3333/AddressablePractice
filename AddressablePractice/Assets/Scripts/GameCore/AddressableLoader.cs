using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLoader : Singleton<AddressableLoader>, IInitializable
{
    public Dictionary<string, IList<ScriptableObject>> loadedData = new();

    public async Task Init()
    {
        await Addressables.InitializeAsync().Task;

        string[] labels = { "Attack", "Defense" }; //라벨목록 관리 이후 안정화 되면 라벨 자동 탐색후 추가하도록 변경할것

        foreach(var label in labels)
        {
            var handle = Addressables.LoadAssetsAsync<ScriptableObject>(label, null);
            IList<ScriptableObject> assets = await handle.Task;
            loadedData[label] = assets;
        }

        Debug.Log("Addressable SO 로드 완료");
    }
}