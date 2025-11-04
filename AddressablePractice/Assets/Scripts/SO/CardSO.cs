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
    public int Value;
    public int Cost;
}

[CreateAssetMenu(fileName = "CardSO", menuName = "SO/CardSO")]
public class CardSO : BaseSO
{
    public CardType cardType;
    public int value;
    public int cost;

    public override void ApplyData(object sheetData)
    {
        if (sheetData is not CardSheetData data) return;

        ApplyBaseData(data);
        
        value = data.Value;
        cost = data.Cost;
        value = data.Value;
        cost = data.Cost;
        Type = "Card"; //임시

        if (Enum.TryParse(data.CardType, true, out CardType parsed))
        {
            cardType = parsed;
        }
        else
        {
            Debug.LogError($"CardSO : {ID} : 잘못된 cardType값 {data.CardType}");
        }
    }
}
