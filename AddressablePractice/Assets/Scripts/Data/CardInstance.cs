using System;
using UnityEngine;

public enum CardState
{
    InLibrary,
    InHand,
    InDiscard,
    InExhaust
}
[Serializable]
public class CardInstance //실제 플레이어가 가진 카드 인스턴스
{
    public string InstanceID; //개별 카드 구분용(덱에서 중복 카드 구분)
    public CardSO Template; //원본 SO

    public int TemporaryCost; //비용 비용이 줄어들수도 있으므로
    public int TemporaryDamage; 
    public int TemporaryShield;

    public CardState State;

    public bool IsUpgraded; //강화여부 이후 여러번 업그레이드 할수있는 카드가 생기면? 바뀔부분

    public CardInstance(CardSO so)
    {
        InstanceID = Guid.NewGuid().ToString(); //고유 인스턴스 ID
        Template = so;

        TemporaryCost = so.cost;
        TemporaryDamage = so.damage;
        TemporaryShield = so.shield;

        State = CardState.InLibrary;
        IsUpgraded = false;
    }
}
