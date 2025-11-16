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
    public string CardType; //카드의 타입
    public string Rarity; //카드 등급
    public int Damage;
    public int Shield;
    public int Cost;
}

[CreateAssetMenu(fileName = "CardSO", menuName = "SO/CardSO")]
public class CardSO : BaseSO
{
    public CardType cardType;
    public RarityType rarity;
    public int damage;
    public int shield;
    public int cost;

    public override void ApplyData(object sheetData)
    {
        if (sheetData is not CardSheetData data) return;

        ApplyBaseData(data);

        damage = data.Damage;
        shield = data.Shield;
        cost = data.Cost;
        Type = "Card"; //임시


        if (Enum.TryParse(data.CardType, true, out CardType parsedCardType))
            cardType = parsedCardType;
        else
            Debug.LogError($"{data.CardType}은 CardType의 형식에 맞지않습니다.");

        if (Enum.TryParse(data.Rarity, true, out RarityType parsedRarity))
            rarity = parsedRarity;
        else
            Debug.LogError($"{data.Rarity}은 RarityType 형식에 맞지않습니다.");

    }
}
