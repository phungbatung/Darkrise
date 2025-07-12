using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventorySaveData 
{
    public int gold;
    public int diamond;
    public int inventorySize;
    public List<ItemInventorySave> inventoryItems;
    public int equipmentSize;
    public List<ItemInventorySave> equipedItems;
    
    public InventorySaveData() 
    {
        // For NewGame
        gold = 100000;
        diamond = 100000;
        inventorySize = 48;
        inventoryItems = new List<ItemInventorySave>();
        for (int i = 0; i < inventorySize; i++)
            inventoryItems.Add(new ItemInventory());
        equipedItems = new List<ItemInventorySave>();
    }
    public InventorySaveData(int _inventorySize, int _gold, int _diamond, List<ItemInventory> _inventoryItems, List<ItemInventory> _equipedItems) 
    { 
        inventorySize = _inventorySize;
        gold = _gold;
        diamond = _diamond;
        inventoryItems = _inventoryItems.Select(item => (ItemInventorySave)item).ToList();
        equipedItems = _equipedItems.Select(item => (ItemInventorySave)item).ToList();
        
    }



    [Serializable]
    public class ItemInventorySave
    {
        public int ItemId;
        public int Amount;
        public EquipmentPropertiesSave EquipmentProperties;

        public ItemInventorySave(int itemId, int amount, EquipmentProperties equipmentProperties)
        {
            ItemId = itemId;
            Amount = amount;
            EquipmentProperties = equipmentProperties;
        }

        public static implicit operator ItemInventory(ItemInventorySave item)
        {
            if(item == null) 
                return new ItemInventory();
            return new ItemInventory(ItemManager.Instance.itemDict[item.ItemId], item.Amount, item.EquipmentProperties);
        }
        public static implicit operator ItemInventorySave(ItemInventory item)
        {
            if (item == null) 
                return new ItemInventory();
            return new ItemInventorySave(item.itemData.id, item.amount, item.equipmentProperties);
        }
    }

    [Serializable]
    public class EquipmentPropertiesSave
    {
        public SerializableDictionary<string, string> BaseProperties;
        public SerializableDictionary<string, string> Properties;

        public int UnlockedGemsSlot;
        public int[] Gems;

        public int EnhanceLevel;

        public EquipmentPropertiesSave(Dictionary<string, string> baseProperties, 
            Dictionary<string, string> properties, int unlockedGemsSlot, int[] gems, int enhanceLevel)
        {
            BaseProperties = new();
            foreach(var kvp in baseProperties)
            {
                this.BaseProperties[kvp.Key] = kvp.Value;
            }
            Properties = new();
            foreach (var kvp in properties)
            {
                this.Properties[kvp.Key] = kvp.Value;
            }
            this.UnlockedGemsSlot = unlockedGemsSlot;
            this.Gems = gems;
            this.EnhanceLevel = enhanceLevel;
        }

        public static implicit operator EquipmentProperties(EquipmentPropertiesSave props)
        {
            if (props == null) 
                return null;
            return new EquipmentProperties(props.BaseProperties, props.Properties, props.UnlockedGemsSlot, props.Gems.Select(id => ItemManager.Instance.BuildInventoryItem(ItemManager.Instance.itemDict[id])).ToArray(), props.EnhanceLevel);
        }

        public static implicit operator EquipmentPropertiesSave(EquipmentProperties props)
        {
            if (props == null)
                return null;
            return new EquipmentPropertiesSave(props.baseProperties, props.properties, props.unlockedGemsSlot, props.gems.Select(obj => obj.itemData.id).ToArray(), props.enhanceLevel);
        }
    }
}
