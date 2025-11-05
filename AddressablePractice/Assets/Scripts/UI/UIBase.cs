using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LayerType //레이어 상단, 중단, 하단 정하려고 만든 Enum
{
    Top,
    Middle,
    Bottom
}
public class UIBase : MonoBehaviour
{
    [Header("UI Layer Setting")]
    [SerializeField] public LayerType layerType = LayerType.Middle;

    protected bool isInit = false;

    /// <summary>
    /// UI 최초 1회 초기화
    /// </summary>
    public virtual void Init()
    {
        if (isInit) return;
        isInit = true;

        OnInit();
    }

    /// <summary>
    /// 실제 초기화 로직은 자식에서 ex) 컴포넌트 연결등
    /// </summary>
    public virtual void OnInit() { }

    /// <summary>
    /// UI 활성화
    /// </summary>
    public virtual void Open()
    {
        if (!isInit) Init();

        gameObject.SetActive(true);

        OnOpen();
    }

    /// <summary>
    /// UI 비활성화
    /// </summary>
    public virtual void Close()
    {
        gameObject.SetActive(false);
        OnClose();
    }

    /// <summary>
    /// 세부적 로직은 자식에서 재정의(Override)
    /// </summary>
    public virtual void OnOpen() { }

    public virtual void OnClose() { }
}
