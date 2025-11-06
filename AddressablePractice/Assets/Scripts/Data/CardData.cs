using System;
using UnityEngine;

[Serializable]
public class CardData //실제 플레이어가 가진 카드 인스턴스
{
    public string UniqueID; //개별 카드 구분용(덱에서 중복 카드 구분)
    public CardSO BaseSO; //원본 SO
    public bool IsUpgraded; //강화여부 이후 여러번 업그레이드 할수있는 카드가 생기면? 바뀔부분
    public bool IsUsed; //사용여부
    public int TemporaryCost; //비용 비용이 줄어들수도 있으므로

    public CardData(CardSO baseSO)
    {
        UniqueID = Guid.NewGuid().ToString(); //고유 인스턴스 ID
        BaseSO = baseSO;
        IsUpgraded = false;
        IsUsed = false;
        TemporaryCost = baseSO.cost;
    }
}
