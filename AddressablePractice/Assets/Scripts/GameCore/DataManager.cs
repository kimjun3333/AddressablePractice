using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Addressable이나 외부에서 불러온 데이터를 Label단위로 통합 관리하는 매니저.
/// </summary>
public class DataManager : Singleton<DataManager>, IInitializable
{
    //[SerializeReference] private Dictionary<string, List<ScriptableObject>> dataByLabel = new();
    [SerializeReference] private Dictionary<string, List<Object>> dataByLabel = new();

    public async Task Init()
    {
        foreach (var kvp in AddressableLoader.Instance.loadedData) //로드된 어드레서블 데이터를 DataManager에 추가
        {
            AddData(kvp.Key, kvp.Value);
        }

        Debug.Log("DataManager 준비 완료");
        await Task.CompletedTask;
    }
    
    /// <summary>
    /// Addressable에서 불러온 데이터 라벨별로 등록
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="label"></param>
    /// <param name="assets"></param>
    public void AddData(string label, IEnumerable<Object> assets)
    {
        if (!dataByLabel.ContainsKey(label))
            dataByLabel[label] = new List<Object>();

            foreach(var asset in assets)
            {
                if (!dataByLabel[label].Contains(asset))
                {
                    dataByLabel[label].Add(asset);
                }
            }

        Debug.Log($"DataManager : [{label}] {dataByLabel[label].Count}개 데이터 추가 완료");
    }

    /// <summary>
    /// 특정 라벨의 리스트를 가져오는 함수
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="label"></param>
    /// <returns></returns>

    public List<T> GetDataByLabel<T>(string label) where T : Object
    {
        if(!dataByLabel.TryGetValue(label, out var list))
            return new List<T>();

        List<T> result = new();
        foreach(var item in list)
        {
            if (item is T tItem)
                result.Add(tItem);
        }
        return result;
    }

    public List<T> GetAllDataOfType<T>() where T : Object
    {
        List<T> result = new();
        foreach(var kvp in dataByLabel)
        {
            foreach(var item in kvp.Value)
            {
                if(item is T  tItem)
                    result.Add(tItem);
            }
        }

        return result;
    }
}
