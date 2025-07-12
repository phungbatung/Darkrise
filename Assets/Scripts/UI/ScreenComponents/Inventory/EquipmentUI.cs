using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    private EquipmentSlot[] itemSlots;
    public List<ItemInventory> equipedItem => ItemManager.Instance.equipedItems;

    private void Awake()
    {
        itemSlots = GetComponentsInChildren<EquipmentSlot>();
    }

    public void UpdateItemSlot()
    {
        for (int i = 0; i < itemSlots.Length; i++) 
        {
            itemSlots[i].UpdateUI(equipedItem[i]);
        }
    }
}
