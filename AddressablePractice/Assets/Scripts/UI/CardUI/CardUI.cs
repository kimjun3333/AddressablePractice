using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [Header("Card UI Components")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button cardButton;

    private CardData cardData;
    private Action<CardData> onClickAction;

    public void SetUp(CardData data, Action<CardData> onClick = null)
    {
        cardData = data;
        onClickAction = onClick;

        var so = data.BaseSO;

        icon.sprite = so.Sprite;
        nameText.text = so.Name;
        descriptionText.text = so.Description;
        costText.text = data.TemporaryCost.ToString();

        cardButton.onClick.RemoveAllListeners();
        if (onClickAction != null)
            cardButton.onClick.AddListener(() => onClickAction(cardData));
    }
}
