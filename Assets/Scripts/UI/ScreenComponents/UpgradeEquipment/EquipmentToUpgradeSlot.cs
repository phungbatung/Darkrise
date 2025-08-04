using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentToUpgradeSlot : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    
    [SerializeField] private Image bg;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI enhanceLevel;
    [SerializeField] private TextMeshProUGUI equipmentName;
    [SerializeField] private TextMeshProUGUI level;

    public Action<ItemInventory> OnDropAction;
    public Action OnPointerDownAction;
    public void OnDrop(PointerEventData eventData)
    {
        ItemSlot slot = eventData.pointerDrag.GetComponent<ItemSlot>();
        ItemData itemData = slot.itemInventory.itemData;
        if (itemData.Type != ItemType.Equipment)
            return;
        SetItem(slot.itemInventory);
        OnDropAction?.Invoke(slot.itemInventory);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetEmtyItem();
        OnPointerDownAction?.Invoke();
    }

    public void SetEmtyItem()
    {
        bg.sprite = AssetManager.Instance.GetItemSlotBackGroundImageByKey(ItemRarity.Common.ToString());
        icon.sprite = null;
        icon.color = new Color(1, 1, 1, 0);
        equipmentName.text = "";
        level.text = "";
    }
    public void SetItem(ItemInventory _itemInventory)
    {
        ItemData itemData = _itemInventory.itemData;
        bg.sprite = AssetManager.Instance.GetItemSlotBackGroundImageByKey(itemData.Rarity.ToString());
        icon.sprite = itemData.Icon;
        enhanceLevel.text = _itemInventory.equipmentProperties.enhanceLevel < 1 ? "" : $"+{_itemInventory.equipmentProperties.enhanceLevel}";
        icon.color = new Color(1, 1, 1, 1);
        equipmentName.text = itemData.Name;
        level.text = $"Lv: {itemData.Level}";
    }
}
