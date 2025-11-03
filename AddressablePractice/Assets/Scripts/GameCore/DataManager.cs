using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Addressable이나 외부에서 불러온 데이터를 Label단위로 통합 관리하는 매니저.
/// </summary>
public class DataManager : Singleton<DataManager>, IInitializable
{
    [SerializeReference] private Dictionary<string, List<ScriptableObject>> dataByLabel = new();

    public async Task Init()
    {
        Debug.Log("DataManager 준비 완료");
        await Task.CompletedTask;
    }
    
    /// <summary>
    /// Addressable에서 불러온 데이터 라벨별로 등록
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="label"></param>
    /// <param name="assets"></param>
    public void AddData<T>(string label, IEnumerable<T> assets) where T : ScriptableObject
    {
        if(!dataByLabel.ContainsKey(label))
        {
            dataByLabel[label] = new List<ScriptableObject>();

            foreach(var asset in assets)
            {
                if (!dataByLabel[label].Contains(asset))
                {
                    dataByLabel[label].Add(asset);
                }
            }

            Debug.Log($"DataManger : {label} {dataByLabel[label].Count}개 데이터 추가 ");
        }
    }

    /// <summary>
    /// 특정 라벨의 리스트를 가져오는 함수
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="label"></param>
    /// <returns></returns>

    public List<T> GetDataByLabel<T>(string label) where T : ScriptableObject
    {
        if(!dataByLabel.TryGetValue(label, out var list))
        {
            return new List<T>();
        }

        return list.ConvertAll(x => x as T);
    }
}
