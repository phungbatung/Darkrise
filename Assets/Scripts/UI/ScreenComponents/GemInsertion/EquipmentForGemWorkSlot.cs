using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentForGemWorkSlot : MonoBehaviour, IPointerDownHandler, IDropHandler
{

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI equipmentName;
    [SerializeField] private TextMeshProUGUI level;

    public Action<ItemInventory> OnDropAction;
    public Action OnPointerDownAction;
    public void OnDrop(PointerEventData eventData)
    {
        ItemSlot slot = eventData.pointerDrag.GetComponent<ItemSlot>();
        ItemData itemData = slot.itemInventory.itemData;
        if (itemData.type != ItemType.Equipment)
            return;
        SetItem(itemData);
        OnDropAction?.Invoke(slot.itemInventory);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetEmtyItem();
        OnPointerDownAction?.Invoke();
    }

    public void SetEmtyItem()
    {
        icon.sprite = null;
        icon.color = new Color(1, 1, 1, 0);
        equipmentName.text = "";
        level.text = "";
    }
    public void SetItem(ItemData itemData)
    {
        icon.sprite = itemData.icon;
        icon.color = new Color(1, 1, 1, 1);
        equipmentName.text = itemData.name;
        level.text = $"Lv: {itemData.level}";
    }    
}
