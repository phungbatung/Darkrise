using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GemSlot : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    public int slotIndex { get; set; }
    public ItemInventory itemInventory { get; set; }

    [Header("Component")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image bg;
    [SerializeField] private TextMeshProUGUI itemProperties;

    [Header("Locked sprite")]
    [SerializeField] private Sprite lockedSprite;
    private string lockedMessage { get; } = "Unlock for 0 gold";

    public Action<int, ItemSlot> OnDropEvent { get; set; }
    public Action<int> PointerDownEvent { get; set; }

    public void SetLocked(int _price)
    {
        itemIcon.sprite = lockedSprite;
        itemProperties.text = $"Unlock for {_price} gold";
    }
    public void SetProperties(ItemData itemData)
    {
        if(itemData == ItemData.Empty)
        {
            itemIcon.color = new Color(1, 1, 1, 0);
            itemIcon.sprite = null;
            bg.sprite = AssetManager.Instance.GetItemSlotBackGroundImageByKey(ItemRarity.Common.ToString());
            itemProperties.text = "Gem socket";
            return;
        }
        itemIcon.sprite = itemData.Icon;
        itemIcon.color = new Color(1, 1, 1, 1);
        bg.sprite = AssetManager.Instance.GetItemSlotBackGroundImageByKey(itemData.Rarity.ToString());
        string properties = "";
        foreach(var property in  itemData.Properties)
        {
            properties += $"+{property.Value} {property.Key}\n";
        }
        itemProperties.text = properties;
    }
    public void OnDrop(PointerEventData eventData)
    {
        ItemSlot itemSlot = eventData.pointerDrag.GetComponent<ItemSlot>();

        if(itemSlot != null)
            OnDropEvent?.Invoke(slotIndex, itemSlot);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDownEvent?.Invoke(slotIndex);
    }
}
