using System;
using UnityEngine;

public enum CardType
{
    Attack,
    Defense,
    Utility
}
/// <summary>
/// 구글시트 한줄의 데이터를 담는 클래스
/// </summary>
[Serializable]
public class CardSheetData : BaseSheetData
{
    public CardType CardType; //카드의 타입
    public RarityType Rarity; //카드 등급
    public int Value;
    public int Cost;
}

[CreateAssetMenu(fileName = "CardSO", menuName = "SO/CardSO")]
public class CardSO : BaseSO
{
    public CardType cardType;
    public RarityType rarity;
    public int value;
    public int cost;

    public override void ApplyData(object sheetData)
    {
        if (sheetData is not CardSheetData data) return;

        ApplyBaseData(data);
        
        value = data.Value;
        cost = data.Cost;
        cardType = data.CardType;
        rarity = data.Rarity;
        Type = "Card"; //임시

        //if (Enum.TryParse(data.CardType, true, out CardType _cardType))
        //{
        //    cardType = _cardType;
        //}
        //else
        //{
        //    Debug.LogError($"CardSO : {ID} : 잘못된 cardType값 {data.CardType}");
        //}

        //if(Enum.TryParse(data.Rarity, true, out RarityType _rarity))
        //{
        //    rarity = _rarity;
        //}
        //else
        //{
        //    Debug.LogError($"CardSO : {ID} : 잘못된 RairtyType값 {data.Rarity}");
        //}
    }
}
