using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseStoreUI : MonoBehaviour
{
    [Header("Store UI")]
    [SerializeField] protected Image icon;
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected TextMeshProUGUI priceText;
    [SerializeField] protected TextMeshProUGUI descriptionText;
    [SerializeField] protected Button buyButton;

    protected object itemData;

    public virtual void Setup(object data)
    {
        ClearUI();
        itemData = data;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuy);
    }

    protected abstract void OnBuy();

    public virtual void ClearUI()
    {
        icon.sprite = null;
        priceText.text = string.Empty;
        descriptionText.text = string.Empty;
        priceText.text = string.Empty;
    }
}
