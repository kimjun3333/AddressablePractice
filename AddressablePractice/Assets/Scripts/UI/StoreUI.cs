using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : UIBase
{
    [SerializeField] private Transform contextParent;
    [SerializeField] private GameObject cardSlotPrefab;
    [SerializeField] private List<CardSO> cardList;

    public override void OnInit()
    {
        foreach (var so in cardList)
        {
            var slotObj = Instantiate(cardSlotPrefab, contextParent);
            var slot = slotObj.GetComponent<CardStoreUI>();
            slot.Setup(so);
            slot.OnBuyRequested = HandleBuyCard;
        }
    }

    private void HandleBuyCard(CardSO card)
    {
        Debug.Log("카드 구매!");
    }
}
