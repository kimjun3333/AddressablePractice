using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStoreUI : BaseStoreUI
{
    private CardSO card;
    public Action<CardSO> OnBuyRequested;

    public override void Setup(object data)
    {
        base.Setup(data);

        card = data as CardSO;

        if (card == null) return;

        icon.sprite = card.Sprite;
        nameText.text = card.Name;
        descriptionText.text = card.Description;
        priceText.text = $"100G"; //미적용

    }
    protected override void OnBuy()
    {
        if (card == null) return;

        if(OnBuyRequested != null)
        {
            OnBuyRequested.Invoke(card);
        }
        else
        {
            Debug.Log($"CardStoreUI : 구매요청 {card.Name}");
        }
    }
}
