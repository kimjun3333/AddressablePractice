using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;
using System.Linq;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Addressable 비동기 로드하는 매니저.
/// </summary>

public class AddressableLoader : Singleton<AddressableLoader>, IInitializable
{
    public Dictionary<string, IList<ScriptableObject>> loadedData = new(); //라벨별로 로드된 SO 저장하는 딕셔너리

    public async Task Init()
    {
        await Addressables.InitializeAsync().Task; //Addressable 시스템 초기화

        HashSet<string> allKeys = new(); //모든 키 수집
        foreach(var locator in Addressables.ResourceLocators)
        {
            foreach(var key in locator.Keys)
            {
                if(key is string sKey)
                {
                    allKeys.Add(sKey);
                }
            }
        }

        List<string> labelCandidates = new(); //키중에서 라벨로 추정되는 후보만 필터링
        foreach(var key in allKeys)
        {
            if (key.Contains("/") || key.Contains(".") || key.StartsWith("guid_", StringComparison.OrdinalIgnoreCase))
                continue;

            if (key.Any(char.IsDigit))
                continue;

            labelCandidates.Add(key);
        }

        Debug.Log($"AddressableLoader : 탐색된 라벨 {labelCandidates.Count}개");

        //필터링된 각 라벨을 대상으로 실제 SO 로드 시도
        foreach(var label in labelCandidates)
        {
            AsyncOperationHandle<IList<ScriptableObject>> handle = default;
            try
            {
                //라벨에 속한 SO 비동기로 로드
                handle = Addressables.LoadAssetsAsync<ScriptableObject>(label, null);
                var assets = await handle.Task;

                //SO가 존재하면 로드
                if(assets != null && assets.Count > 0)
                {
                    loadedData[label] = assets;
                    Debug.Log($"AddressableLoader : 라벨 {label} -> {assets.Count}개 로드됨");
                }
            }
            finally
            {
                //핸들이 유효하면 Release로 해제 (메모리 누수 방지)
                if(handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
        }

        Debug.Log($"AddressableLoader : Addressable SO 로드 완료 ({loadedData.Count}개 라벨)");
    }
}