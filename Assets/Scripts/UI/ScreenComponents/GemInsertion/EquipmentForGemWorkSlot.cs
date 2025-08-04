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
    [SerializeField] private Image bg;
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
        bg.sprite = AssetManager.Instance.GetItemSlotBackGroundImageByKey(ItemRarity.Common.ToString());
        equipmentName.text = "";
        level.text = "";
    }
    public void SetItem(ItemData itemData)
    {
        icon.sprite = itemData.Icon;
        icon.color = new Color(1, 1, 1, 1);
        bg.sprite = AssetManager.Instance.GetItemSlotBackGroundImageByKey(itemData.Rarity.ToString());
        equipmentName.text = itemData.Name;
        level.text = $"Lv: {itemData.Level}";
    }    
}
