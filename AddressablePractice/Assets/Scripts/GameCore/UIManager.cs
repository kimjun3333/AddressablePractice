using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>, IInitializable //해상도 대응 해야함
{
    private readonly Dictionary<string, UIBase> activeUI = new(); //현재 활성화된 UI
    private readonly Dictionary<string, UIBase> allUI = new(); //생성된 모든 UI
    private readonly Dictionary<LayerType, Transform> layerRoots = new(); //Layer별 Canvas
    private readonly Dictionary<string, GameObject> uiPrefabs = new(); //UI 프리팹 캐시

    private Transform uiRoot; //모든 Canvas를 담은 최상위 UI루트

    public async Task Init()
    {
        Debug.Log("UIManager 초기화 시작");

        GameObject rootObj = new GameObject("UI"); //UI루트 생성

        uiRoot = rootObj.transform;

        foreach(LayerType layer in Enum.GetValues(typeof(LayerType)))
        {
            if (!layerRoots.ContainsKey(layer))
                layerRoots.Add(layer, CreateLayerRoot(layer, uiRoot));
        }

        List<GameObject> uiList = DataManager.Instance.GetDataByLabel<GameObject>("UI");
        foreach(var ui in uiList)
        {
            if(ui != null && !uiPrefabs.ContainsKey(ui.name))
            {
                uiPrefabs.Add(ui.name, ui);
                Debug.Log($"UIManager : {ui.name}프리팹 등록");
            }
        }

        foreach(var kvp in uiPrefabs)
        {
            var prefab = kvp.Value;
            var uiBase = prefab.GetComponent<UIBase>();
            if (uiBase == null) continue;

            var instance = Instantiate(prefab, layerRoots[uiBase.layerType]);
            instance.name = prefab.name;


            var ui = instance.GetComponent<UIBase>();
            ui.Init();
            ui.Close();

            allUI.Add(prefab.name, ui);
        }

        Debug.Log($"UIMAnager : 등록된 프리팹 수 {uiPrefabs.Count}");
        await Task.CompletedTask;
    }

    public T Open<T>(string id = null) where T : UIBase
    {
        string key = id ?? typeof(T).Name;

        if(!allUI.TryGetValue(key, out var ui))
        {
            Debug.LogError($"UIManager : {key} UI가 Init시 생성되지 않았음");
            return null;
        }

        if(activeUI.ContainsKey(key))
            return (T)activeUI[key];

        ui.Open();
        activeUI[key] = ui;
        return (T)ui;
    }

    public void Close<T>(string id = null) where T : UIBase
    {
        string key = id ?? typeof(T).Name;

        if(activeUI.TryGetValue(key, out var ui))
        {
            ui.Close();
            activeUI.Remove(key);
        }
    }

    private Transform CreateLayerRoot(LayerType layerType, Transform parent)
    {
        GameObject root = new($"{layerType}Canvas");
        root.transform.SetParent(parent, false);
        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = (int)layerType;

        CanvasScaler scaler = root.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;
       
        root.AddComponent<GraphicRaycaster>();

        return root.transform;
    }
}
