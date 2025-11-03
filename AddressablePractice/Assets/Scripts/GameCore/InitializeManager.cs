using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 초기화 매니저 초기화 순서를 조절하기 위한 매니저.
/// </summary>
public class InitializeManager : Singleton<InitializeManager>
{
    private async void Awake()
    {
        Debug.Log("초기화 시작");
        await AddressableLoader.Instance.Init();
        await DataManager.Instance.Init();

        foreach(var kvp in AddressableLoader.Instance.loadedData)
        {
            DataManager.Instance.AddData(kvp.Key, kvp.Value);
        }

        await GoogleLoader.Instance.Init();

        Debug.Log("모든 초기화 완료");
    }
}
