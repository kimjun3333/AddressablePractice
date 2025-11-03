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
[System.Serializable]
public class CardSheetData
{
    public string ID; 
    public string cardName;
    public string cardType; //카드의 타입
    public int value;
    public string description;
    public int cost; 

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
        Name = data.cardName;
        Description = data.description;
        value = data.value;
        cost = data.cost;

        if (value != data.value)
        {
            Debug.Log($"[CardSO] {ID}: value {value} → {data.value}");
            value = data.value;
        }

        if (cost != data.cost)
        {
            Debug.Log($"[CardSO] {ID}: cost {cost} → {data.cost}");
            cost = data.cost;
        }

        if (System.Enum.TryParse(data.cardType, true, out CardType parsed))
        {
            cardType = parsed;
        }
        else
        {
            Debug.LogError($"CardSO : {ID} : 잘못된 cardType값 {data.cardType}");
        }
    }
}
