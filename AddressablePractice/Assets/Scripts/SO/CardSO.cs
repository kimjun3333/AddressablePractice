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

        ID = data.ID;
        Name = data.Name;
        Description = data.Description;
        value = data.Value;
        cost = data.Cost;
        Type = "Card"; //임시

        if (value != data.Value)
        {
            Debug.Log($"[CardSO] {ID}: value {value} → {data.Value}");
            value = data.Value;
        }

        if (cost != data.Cost)
        {
            Debug.Log($"[CardSO] {ID}: cost {cost} → {data.Cost}");
            cost = data.Cost;
        }

        if (System.Enum.TryParse(data.CardType, true, out CardType parsed))
        {
            cardType = parsed;
        }
        else
        {
            Debug.LogError($"CardSO : {ID} : 잘못된 cardType값 {data.CardType}");
        }
    }
}
