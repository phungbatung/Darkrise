using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemInventory
{
    public ItemData itemData;
    public int amount;
    public EquipmentProperties equipmentProperties;


    public ItemInventory()
    {
        itemData = ItemData.Empty;
        amount = 0;
    }

    public ItemInventory(ItemData _itemData, int _amount, EquipmentProperties _equipmentProperties)
    {
        itemData = _itemData;
        amount = _amount;
        equipmentProperties = _equipmentProperties;
    }

    public void AddItem(ItemData _itemData, int _amount = 1)
    {
        if (this.IsEmpty())
            itemData = _itemData;
        amount += _amount;
    }
    public void AddItem(ItemData _itemData, Dictionary<string, string> _properties = null)
    {
        if (IsEmpty())
            itemData = _itemData;

        if (_properties != null)
        {
            equipmentProperties = new(ItemUtilities.GetBaseProperties(_itemData) ,_properties);
        }
        amount++;
    }
    public void RemoveItem(int _amount = 1)
    {
        amount -= _amount;
        if (amount <= 0)
            itemData = null;
        if (amount <= 0)
        {
            itemData = null;
            equipmentProperties = null;
        }
    }
    public void RemoveAll()
    {
        amount = 0;
        itemData = null;
        equipmentProperties = null;
    }
    public bool IsEmpty()
    {
        return itemData == ItemData.Empty;
    }
    public bool CanBeAdded(ItemData _itemId, int _addAmount = 1)
    {
        return itemData == _itemId && amount + _addAmount <= itemData.maxSize;
    }

    public static void Swap(ref ItemInventory item1, ref ItemInventory item2)
    {
        ItemInventory temp = item1;
        item1 = item2;
        item2 = temp;
    }
    public static void SwapValue(ItemInventory item1, ItemInventory item2)
    {
        if(item1 == null)
        {
            Debug.Log("item1 is null");
        }
        if (item2 == null)
        {
            Debug.Log("item2 is null");
        }
        (item1.itemData, item2.itemData) = (item2.itemData, item1.itemData);
        (item1.amount, item2.amount) = (item2.amount, item1.amount);
        (item1.equipmentProperties, item2.equipmentProperties) = (item2.equipmentProperties, item1.equipmentProperties);
    }
    public void Clone(ItemInventory _itemInventory)
    {
        itemData = _itemInventory.itemData;
        amount = _itemInventory.amount;
        equipmentProperties = _itemInventory.equipmentProperties;
    }
    public static int CompareByItemType(ItemInventory itemInventory1, ItemInventory itemInventory2)
    {
        if (itemInventory1.IsEmpty() && !itemInventory2.IsEmpty()) return 1;
        if (!itemInventory1.IsEmpty() && itemInventory2.IsEmpty()) return -1;
        if (itemInventory1.IsEmpty() && itemInventory2.IsEmpty()) return 0;
        ItemData item1 = itemInventory1.itemData;
        ItemData item2 = itemInventory2.itemData;
        if (item1.type > item2.type) return 1;
        if (item1.type < item2.type) return -1;
        if (item1.rarity < item2.rarity) return 1;
        if (item1.rarity > item2.rarity) return -1;
        return 0;
    }
    public static int CompareByItemQuality(ItemInventory itemInventory1, ItemInventory itemInventory2)
    {
        if (itemInventory1.IsEmpty() && !itemInventory2.IsEmpty()) return 1;
        if (!itemInventory1.IsEmpty() && itemInventory2.IsEmpty()) return -1;
        if (itemInventory1.IsEmpty() && itemInventory2.IsEmpty()) return 0;
        ItemData item1 = itemInventory1.itemData;
        ItemData item2 = itemInventory2.itemData;
        if (item1.rarity < item2.rarity) return 1;
        if (item1.rarity > item2.rarity) return -1;
        if (item1.type > item2.type) return 1;
        if (item1.type < item2.type) return -1;
        return 0;
    }
}
