using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Card UI Components")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;

    private CardInstance instance;

    //외부에서 UI 세팅하는 함수
    public void Bind(CardInstance card)
    {
        instance = card;
        UpdateUI();
    }

    public void UpdateUI()
    {
        var so = instance.Template;

        icon.sprite = so.Sprite;
        nameText.text = so.Name;
        descriptionText.text = so.Description;
        costText.text = instance.TemporaryCost.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{instance.CardName} 클릭됨");
    }

    public CardInstance GetInstance()
    {
        return instance;
    }
}
