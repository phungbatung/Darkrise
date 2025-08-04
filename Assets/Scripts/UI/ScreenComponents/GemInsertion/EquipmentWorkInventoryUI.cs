using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentWorkInventoryUI : InventoryUI
{
    public ItemInventory onWorkItem;
    public Action itemSlotPoiterClickEvent;
    public override void UpdateItemSlot()
    {
        int slotCount = currentPage != getTotalPage() ? slotsPerPage : (getInventorySize() - 24 * (currentPage - 1));

        for (int i = 0; i < slotsPerPage; i++)
        {
            if (i < slotCount)
                itemSlots[i].gameObject.SetActive(true);
            else
                itemSlots[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < slotCount; i++)
        {
            //Debug.Log((currentPage - 1) * 24 + i);
            itemSlots[i].SetItem(inventoryItem[(currentPage - 1) * 24 + i]);
            itemSlots[i].alternativeClickAction = null;
            if (onWorkItem != null && itemSlots[i].itemInventory == onWorkItem)
            {
                itemSlots[i].SetBlur(true);
                itemSlots[i].alternativeClickAction = itemSlotPoiterClickEvent;
            }
        }
        pageCountTMP.text = $"{currentPage}/{getTotalPage()}";
        SetUpButton();
    }

    public void SetItemToWorkOnGem(ItemInventory _itemInventory, Action _itemSlotPoiterClickEvent = null)
    {
        onWorkItem = _itemInventory;
        itemSlotPoiterClickEvent = _itemSlotPoiterClickEvent;
        ItemSlot slot = itemSlots.First(o => o.itemInventory == _itemInventory);
        slot.SetBlur(true);
        slot.alternativeClickAction = itemSlotPoiterClickEvent;
    }
    public void UpdateOnWorkItemSlot()
    {
        ItemSlot slot = itemSlots.FirstOrDefault(o => o.itemInventory == onWorkItem);
        if (slot != null)
        {
            slot.UpdateUI();
            slot.SetBlur(true);
        }
    }
    public void RemoveOnWorkItem()
    {
        if (onWorkItem == null || onWorkItem.IsEmpty())
            return;
        ItemSlot slot = itemSlots.First(o => o.itemInventory == onWorkItem);
        slot.SetBlur(false);
        slot.alternativeClickAction = null;
        onWorkItem = null;
    }    
}
