using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 초기화 매니저 초기화 순서를 조절하기 위한 매니저.
/// </summary>
public class InitializeManager : Singleton<InitializeManager>
{
    protected override async void Awake()
    {   
        base.Awake();

        Debug.Log("초기화 시작");
        await AddressableLoader.Instance.Init(); //어드레서블 데이터 불러오기
        await GoogleLoader.Instance.Init(); //어드레서블 데이터 덮어쓰기 
        AddressableLoader.Instance.LinkAllSprites(); //데이터 덮어쓴 이후에 스프라이트 ID 비교후 연결해주기
        await DataManager.Instance.Init(); //Addressable에서 DataManager로 데이터 옮기는과정을 DataManager Init으로 옮김  
        await UIManager.Instance.Init();

        Debug.Log("모든 초기화 완료");
    }
}
