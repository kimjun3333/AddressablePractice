using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLoader : MonoBehaviour
{
    private async void Start()
    {
        await Addressables.InitializeAsync().Task;

        var attackHandle = Addressables.LoadAssetsAsync<CardSO>("Attack", null);
        IList<CardSO> attackCards = await attackHandle.Task;
        DataManager.Instance.AddData("Attack", attackCards);

        var defenseHandle = Addressables.LoadAssetsAsync<CardSO>("Defense", null);
        IList<CardSO> defenseCards = await defenseHandle.Task;
        DataManager.Instance.AddData("Defense", defenseCards);

        Debug.Log("모든 라벨 로드 및 등록 완료");
    }
}