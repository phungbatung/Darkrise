using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using JetBrains.Annotations;
using System.Collections.Generic;
using System;
using Unity.Mathematics;
using BlitzyUI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public ItemInventory itemInventory;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI enhanceLevel;

    public Action alternativeClickAction;
    public void UpdateUI()
    {
        if (itemInventory.IsEmpty())
        {
            backgroundImage.sprite = AssetManager.Instance.GetItemSlotBackGroundImageByKey(ItemRarity.Common.ToString());
            itemImage.color = new Color(1, 1, 1, 0);
            itemImage.sprite = null;
            amountText.text = "";
            enhanceLevel.text = "";
        }
        else
        {
            backgroundImage.sprite = AssetManager.Instance.GetItemSlotBackGroundImageByKey(itemInventory.itemData.Rarity.ToString());
            itemImage.color = new Color(1, 1, 1, 1);
            itemImage.sprite = itemInventory.itemData.Icon;
            if (itemInventory.amount <= 1)
                amountText.text = "";
            else
                amountText.text = itemInventory.amount.ToString();
            if (itemInventory.itemData.Type == ItemType.Equipment)
            {
                enhanceLevel.text = itemInventory.equipmentProperties.enhanceLevel < 1 ? "" : $"+{itemInventory.equipmentProperties.enhanceLevel}";
            }
            else
            {
                enhanceLevel.text = "";
            }
        }
    }

    public void SetItem(ItemInventory _itemInventory)
    {
        itemInventory = _itemInventory;
        UpdateUI();
    }

    protected virtual Sprite GetDefaultBackGround()
    {
        return AssetManager.Instance.GetItemSlotBackGroundImageByKey(ItemRarity.Common.ToString());
    }    

    public void SetBlur(bool isBlur)
    {
        if (isBlur)
        {
            float blurValue = 130f / 255f;
            itemImage.color = new Color(blurValue, blurValue, blurValue);
        }
        else
        {
            itemImage.color = new Color(1, 1, 1);
        }    
    }    

    public bool IsEmpty()
    {
        return itemInventory.IsEmpty();
    }

    public static void SwapItemSlot(ItemSlot slot1, ItemSlot slot2)
    {
        ItemInventory temp = slot1.itemInventory;
        slot1.itemInventory = slot2.itemInventory;
        slot2.itemInventory = temp;

        slot1.UpdateUI();
        slot2.UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging)
            return;
        if (itemInventory.IsEmpty())
            return;
        if (alternativeClickAction != null)
        {
            alternativeClickAction();
            alternativeClickAction = null;
            return;
        }
        BlitzyUI.Screen.Data data = new BlitzyUI.Screen.Data();
        data.Add("ItemInventory", itemInventory);
        UIManager.Instance.QueuePush(GameManager.itemInfoScreen, data, null, null);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemImage.raycastTarget = false;
        Transform tempParent = GetComponentInParent<BlitzyUI.Screen>().transform;
        if (tempParent == null)    
            Debug.LogError("The screen parent of this item slot is not found!");
        itemImage.transform.SetParent(tempParent);
        itemImage.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemInventory.IsEmpty())
            return;
        itemImage.transform.position += (Vector3)eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemImage.raycastTarget = true;
        itemImage.transform.SetParent(this.transform);
        RectTransform rectTransform = itemImage.transform as RectTransform;
        rectTransform.anchoredPosition = Vector3.zero;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        ItemSlot toDropSlot = eventData.pointerDrag.GetComponent<ItemSlot>();
        ItemManager inventory = ItemManager.Instance;
        if (toDropSlot.itemInventory.IsEmpty())
            return;
        //Case: slot to drop is equipment slot
        if (ItemManager.Instance.equipedItems.Contains(itemInventory)) 
        {
            //Two equipped item are not the same type
            if (inventory.equipedItems.Contains(toDropSlot.itemInventory))
                return;
            //Cannot drop non-equipment to equipment slot
            if (toDropSlot.itemInventory.itemData.Type != ItemType.Equipment)
                return;
            //Cannot drop equipment of different type in this equipment slot
            if (inventory.GetEquipmentTypeById(toDropSlot.itemInventory.itemData.Id) != inventory.equipedItems.IndexOf(itemInventory))
                return;
            //Equip
            inventory.EquipItem(toDropSlot.itemInventory);
            toDropSlot.UpdateUI();
            UpdateUI();
            return;
        }

        //Case: slot to drop is inventory slot
        //Check if to drop slot is from equipment slot
        if (ItemManager.Instance.equipedItems.Contains(toDropSlot.itemInventory))
        {
            //if slot to drop is empty then move item from equipment slot to this slot
            if (itemInventory.IsEmpty())
            {
                inventory.UnequipItem(toDropSlot.itemInventory, itemInventory);
                toDropSlot.UpdateUI();
                UpdateUI();
                return;
            }
            //Cannot swap item from equipment slot to none equipment
            if (itemInventory.itemData.Type != ItemType.Equipment)
                return;
            //Cannot swap item from equipment slot to another equipment of different type
            if (inventory.GetEquipmentTypeById(itemInventory.itemData.Id) != inventory.GetEquipmentTypeById(toDropSlot.itemInventory.itemData.Id))
                return;
            //Equip this item if it is equipment of the same type
            inventory.EquipItem(itemInventory);
            toDropSlot.UpdateUI();
            UpdateUI();
            return;
        }
        //Swap two item from inventory
        SwapItemSlot(this, toDropSlot);
    }

    public static int CompareByItemId(ItemSlot slot1, ItemSlot slot2)
    {
        return ItemInventory.CompareByItemType(slot1.itemInventory, slot2.itemInventory);
    }
    public static int CompareByItemQuality(ItemSlot slot1, ItemSlot slot2)
    {
        return ItemInventory.CompareByItemQuality(slot1.itemInventory, slot2.itemInventory);
    }
}
