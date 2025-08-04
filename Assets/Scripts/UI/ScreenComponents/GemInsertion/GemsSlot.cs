using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsSlot : MonoBehaviour
{
    private GemInsertionScreen screen;
    private ItemInventory itemInventory;
    private GemSlot[] gemsSlot;
    private void Awake()
    {
        InitGemsSlot();
        screen = GetComponentInParent<GemInsertionScreen>();
    }
    private void InitGemsSlot()
    {
        gemsSlot = GetComponentsInChildren<GemSlot>();
        for (int i=0; i<gemsSlot.Length; i++)
        {
            gemsSlot[i].slotIndex = i;
            gemsSlot[i].OnDropEvent += PutGemToSlot;
            gemsSlot[i].PointerDownEvent += PointerDownEvent;
        }
    }
    public void SetupGemsSlot(ItemInventory _itemInventory)
    {
        itemInventory = _itemInventory;
        for (int i = 0; i < gemsSlot.Length; i++)
        {
            if(i<itemInventory.equipmentProperties.unlockedGemsSlot)
            {
                //Debug.Log($"{itemInventory.equipmentProperties.gems[i] == ItemData.Empty}, {itemInventory.equipmentProperties.gems[i].Id}");
                gemsSlot[i].SetProperties(itemInventory.equipmentProperties.gems[i]);
                gemsSlot[i].gameObject.SetActive(true);
            }
            else if (i == itemInventory.equipmentProperties.unlockedGemsSlot)
            {
                gemsSlot[i].SetLocked(itemInventory.equipmentProperties.GetUnlockGemSlotPrice().Value);
                gemsSlot[i].gameObject.SetActive(true);
            }
            else
            {
                gemsSlot[i].gameObject.SetActive(false);
            }

        }
    }
    public void UnlockGemSlot()
    {
        itemInventory.equipmentProperties.TryUnlockGemSlot();
        SetupGemsSlot(itemInventory);
    }
    public void PutGemToSlot(int _slotIndex, ItemSlot itemSlot)
    {
        if (itemSlot.itemInventory.itemData.Type != ItemType.Gem)
            return;
        if (itemInventory.equipmentProperties.TryPutGemToSlot(_slotIndex, itemSlot.itemInventory))
        {
            gemsSlot[_slotIndex].SetProperties(itemSlot.itemInventory.itemData);
            itemSlot.UpdateUI();
            SetupGemsSlot(itemInventory);
        }
    }
    public void PointerDownEvent(int _slot)
    {
        if (_slot == itemInventory.equipmentProperties.unlockedGemsSlot)
        {
            UnlockGemSlot();
            return;
        }
        if (itemInventory.equipmentProperties.TryRemoveGemFromSlot(_slot))
        {
            SetupGemsSlot(itemInventory);
            screen.UpdateInventory();
        }    
    }    
}
