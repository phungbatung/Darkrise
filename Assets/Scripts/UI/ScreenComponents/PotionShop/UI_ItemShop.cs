using BlitzyUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemShop : MonoBehaviour, IPointerClickHandler
{
    private ItemShop itemShop;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image currencyIcon;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button purchaseButton;

    public void SetItem(ItemShop _itemShop)
    {
        itemShop = _itemShop;
        ItemData itemData = itemShop.item.itemData;
        itemIcon.sprite = itemData.Icon;
        currencyIcon.sprite = Utils.GetCurrencyIcon().IconDict[_itemShop.price.Key];
        priceText.text = Utils.ConvertToKMB(itemShop.price.Value);
        purchaseButton.onClick.AddListener(PurchaseItem);
        gameObject.SetActive(true);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        BlitzyUI.Screen.Data data = new BlitzyUI.Screen.Data();
        data.Add("ItemInventory", itemShop.item);
        UIManager.Instance.QueuePush(GameManager.itemInfoScreen, data);
    }

    public void PurchaseItem()
    {
        if (itemShop.PurchaseThisItem())
        {
            //Destroy(gameObject);
        }
    }
}
