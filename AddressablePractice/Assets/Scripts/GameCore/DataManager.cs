using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager : Singleton<DataManager>, IInitializable
{
    [SerializeReference] private Dictionary<string, List<ScriptableObject>> dataByLabel = new();

    public async Task Init()
    {
        Debug.Log("DataManager 준비 완료");
        await Task.CompletedTask;
    }
    
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

    public List<T> GetDataByLabel<T>(string label) where T : ScriptableObject
    {
        if(!dataByLabel.TryGetValue(label, out var list))
        {
            return new List<T>();
        }

        return list.ConvertAll(x => x as T);
    }
}
