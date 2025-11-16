using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : UIBase
{
    [SerializeField] private Transform contextParent;
    [SerializeField] private CardUI cardPrefab;
    [SerializeField] private List<CardSO> cardList;

    public override void OnInit()
    {
        foreach (var so in cardList)
        {
            //var slotObj = Instantiate(cardSlotPrefab, contextParent);
            //var slot = slotObj.GetComponent<CardStoreUI>();
            //slot.Setup(so);
            //slot.OnBuyRequested = HandleBuyCard;

            var data = new CardInstance(so);

            var cardUI = Instantiate(cardPrefab, contextParent);
            cardUI.SetUp(data, OnBuyCard);
        }
    }

    private void OnBuyCard(CardInstance card)
    {
        Debug.Log($"{card.Template.Name} 구매!");
        // 구매 로직 나중에 추가 예정
    }

    //private void HandleBuyCard(CardSO card)
    //{
    //    Debug.Log("카드 구매!");
    //}
}
