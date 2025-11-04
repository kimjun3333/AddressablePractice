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
    public Dictionary<string, IList<UnityEngine.Object>> loadedData = new(); //SO뿐만아니라 Sprite, Audio, prefab등 다 가능하게.

    private readonly Dictionary<string, Type[]> labelTypeMap = new() //라벨 생길때마다 추가해야 되는 부분 // 가능하면 라벨 자동 탐색기능 찾아보기
    {
        { "Card", new[] { typeof(ScriptableObject) } },
        { "Sprites", new[] { typeof(Sprite) } },
        { "Artifact", new[] { typeof(ScriptableObject)} },
    };

    public async Task Init()
    {
        await Addressables.InitializeAsync().Task; //Addressable 초기화

        Debug.Log("AddressableLoader : Addressables 초기화 완료");

        foreach(var kvp in labelTypeMap)
        {
            string label = kvp.Key;
            Type[] types = kvp.Value;

            foreach(var type in types)
            {
                var method = GetType().GetMethod(nameof(TryLoadLabel),
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                var generic = method.MakeGenericMethod(type);
                await (Task)generic.Invoke(this, new object[] { label });
            }
        }

        Debug.Log($"AddressableLoader : 라벨별 로드 완료 ({loadedData.Count}개 라벨)");
    }

    private async Task TryLoadLabel<T>(string label) where T : UnityEngine.Object
    {
        AsyncOperationHandle<IList<T>> handle = default;
        try
        {
            handle = Addressables.LoadAssetsAsync<T>(label, null);
            var assets = await handle.Task;

            if (assets != null && assets.Count > 0)
            {
                if (!loadedData.ContainsKey(label))
                    loadedData[label] = new List<UnityEngine.Object>();

                foreach (var asset in assets)
                    loadedData[label].Add(asset);

                Debug.Log($"AddressableLoader : [{typeof(T).Name}] 라벨 {label} → {assets.Count}개 로드됨");
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"AddressableLoader : [{typeof(T).Name}] 라벨 {label} 로드 실패 - {ex.Message}");
        }
        finally
        {
            if (handle.IsValid())
                Addressables.Release(handle);
        }
    }

    public void LinkAllSprites()
    {
        if(!loadedData.TryGetValue("Sprites", out var spriteObjs))
        {
            Debug.LogWarning("Addressableloader : 스프라이트 라벨이 존재하지 않음");
            return;
        }

        var sprites = spriteObjs.OfType<Sprite>().ToList();

        foreach(var kvp in loadedData)
        {
            if (kvp.Key == "Sprites") continue;

            foreach(var so in kvp.Value.OfType<BaseSO>())
            {
                if(string.IsNullOrEmpty(so.SpriteID)) continue;

                var found = sprites.FirstOrDefault(s => s.name == so.SpriteID);
                if(found != null)
                {
                    so.Sprite = found;
                }
                else
                {
                    Debug.LogWarning($"AddressableLoader : {so.name} : SpriteID '{so.SpriteID}' 스프라이트 없음");
                }
            }
        }

        Debug.Log($"AddressableLoader : 모든 SO에 Sprite 연결 완료");
    }
}