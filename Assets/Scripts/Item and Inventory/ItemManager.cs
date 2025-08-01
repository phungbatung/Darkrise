using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemManager : MonoBehaviour, ISaveManager
{
    public static ItemManager Instance;

    public ItemDatabase itemDatabase;

    public Dictionary<int, ItemData> itemDict = new Dictionary<int, ItemData>();

    //Inventory
    public int inventorySize { get; private set; }
    public List<ItemInventory> inventoryItems { get; set; }
    public Action OnInventoryItemsChange { get; set; }

    //Equipment
    public int equipmentSize = 8;
    public List<ItemInventory> equipedItems { get; set; }
    public Currency Currency { get; private set; }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        //InitialInventory();
        GenerateItemDataDictionary();
    }

    private void InitialInventory()
    {
        inventoryItems = new();
        equipedItems = new();
        for (int i =0; i < inventorySize; i++)
        {
            inventoryItems.Add(new ItemInventory());
        }
        for (int i = 0; i < equipmentSize; i++)
        {
            equipedItems.Add(new ItemInventory());
        }
    }

    public void AddItem(ItemData _itemData, Dictionary<string, string> properties = null)
    {        
        ItemInventory firstEmptySlot = null;
        foreach (ItemInventory itemInventory in inventoryItems)
        {
            if (itemInventory.CanBeAdded(_itemData))
            {
                itemInventory.AddItem(_itemData, properties);
                OnInventoryItemsChange?.Invoke();
                //if (_itemId == InputManager.Instance.potionSlot.itemId)
                //    InputManager.Instance.potionSlot.UpdateUI();
                return;
            }
            if (itemInventory.IsEmpty() && firstEmptySlot == null)
                firstEmptySlot = itemInventory;
        }

        if (firstEmptySlot!= null)
        {
            firstEmptySlot.AddItem(_itemData, properties);
            OnInventoryItemsChange?.Invoke();
            //if (_itemId == InputManager.Instance.potionSlot.itemId)
            //    InputManager.Instance.potionSlot.UpdateUI();
            return;
        }

        Debug.Log("Inventory is full.");
    }
    public bool TryAddItem(ItemData _itemData, Dictionary<string, string> properties = null)
    {
        ItemInventory firstEmptySlot = null;
        foreach (ItemInventory itemInventory in inventoryItems)
        {
            if (itemInventory.CanBeAdded(_itemData))
            {
                itemInventory.AddItem(_itemData, properties);
                OnInventoryItemsChange?.Invoke();
                return true;
            }
            if (itemInventory.IsEmpty() && firstEmptySlot == null)
                firstEmptySlot = itemInventory;
        }

        if (firstEmptySlot != null)
        {
            firstEmptySlot.AddItem(_itemData, properties);
            OnInventoryItemsChange?.Invoke();
            return true;
        }

        Debug.Log("Inventory is full.");
        return false;

    }    
    public bool TryAddItem(ItemInventory _item)
    {
        ItemInventory firstEmptySlot = null;
        foreach (ItemInventory itemInventory in inventoryItems)
        {
            if (itemInventory.CanBeAdded(_item.itemData, _item.amount))
            {
                itemInventory.AddItem(_item.itemData, _item.amount);
                OnInventoryItemsChange?.Invoke();
                return true;
            }
            if (itemInventory.IsEmpty() && firstEmptySlot == null)
                firstEmptySlot = itemInventory;
        }

        if (firstEmptySlot != null)
        {
            firstEmptySlot.Clone(_item);
            OnInventoryItemsChange?.Invoke();
            return true;
        }
        Debug.Log("Inventory is full.");
        return false;
    }
    public bool TryAddItem(ItemInventory item, int index)
    {
        if (inventoryItems[index].IsEmpty())
        {
            inventoryItems[index] = item;
            return true;
        }
        return false;
    }    

    public bool CanBeAdded(ItemData _itemData)
    {
        foreach (ItemInventory itemInventory in inventoryItems)
        {
            if (itemInventory.CanBeAdded(_itemData) || itemInventory.IsEmpty())
            {
                return true;
            }    
        }
        return false;
    }    

    public void RemoveItem(ItemInventory itemInventory)
    {
        itemInventory.RemoveItem();
    }


    public List<ItemData> GetAllItemOfType(ItemType type)
    {
        return itemDatabase.itemList.Where(item => item.type == type).ToList();
    }

    public List<ItemData> GetAllItemOfType(ItemType[] types)
    {
        var typeSet = new HashSet<ItemType>(types);
        return itemDatabase.itemList
                          .Where(item => typeSet.Contains(item.type))
                          .ToList();
    }

    public List<ItemData> GetAllItemOfType(List<ItemType> types)
    {
        var typeSet = new HashSet<ItemType>(types);
        return itemDatabase.itemList
                          .Where(item => typeSet.Contains(item.type))
                          .ToList();
    }

    public ItemInventory BuildInventoryItem(ItemData itemData)
    {
        ItemInventory _itemInventory = new ItemInventory();
        if (itemData.type == ItemType.Equipment)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            for (int i = 1; i <= itemData.rarity.GetHashCode(); i++)
            {
                int idx = UnityEngine.Random.Range(0, itemData.properties.Count);
                var kvp = itemData.properties.ElementAt(idx);
                if (properties.ContainsKey(kvp.Key))
                {
                    properties[kvp.Key] += $",{(int)UnityEngine.Random.Range(float.Parse(kvp.Value) * .7f, float.Parse(kvp.Value) * 1.3f)}";
                }
                else
                {
                    properties[kvp.Key] = ((int)UnityEngine.Random.Range(float.Parse(kvp.Value) * .7f, float.Parse(kvp.Value) * 1.3f)).ToString();
                }
            }
            _itemInventory.AddItem(itemData, properties);
        }
        else
        {
            _itemInventory.AddItem(itemData, 1);
        }    
        return _itemInventory;
    }    
    #region Equipment
    public void EquipItem(ItemInventory _itemToEquip)
    {
        if (_itemToEquip == null || _itemToEquip.IsEmpty())
            return;
        if (_itemToEquip.itemData.type != ItemType.Equipment)
            return;

        ItemInventory _itemToUnequip = null;
        _itemToUnequip = equipedItems[(int)_itemToEquip.itemData.EquipmentType];
        PlayerManager.Instance.player.stats.AddModifier(_itemToEquip.equipmentProperties.GetAllProperties());
        if (!_itemToUnequip.IsEmpty())
            PlayerManager.Instance.player.stats.RemoveModifier(_itemToUnequip.equipmentProperties.GetAllProperties());
        ItemInventory.SwapValue(_itemToEquip, _itemToUnequip);
    }

    public void UnequipItem(ItemInventory _itemToUnequip, ItemInventory _slotToGiveBack = null )
    {
        if (_itemToUnequip.itemData.type != ItemType.Equipment && !equipedItems.Contains(_itemToUnequip))
            return;

        //Find first emptys
        if (_slotToGiveBack == null)
        {
            foreach (ItemInventory itemInventory in inventoryItems)
            {
                if (itemInventory.IsEmpty())
                {
                    _slotToGiveBack = itemInventory;
                    break;
                }
            }
        }
        if (_slotToGiveBack != null)
        {
            PlayerManager.Instance.player.stats.RemoveModifier(_itemToUnequip.equipmentProperties.GetAllProperties());
            ItemInventory.SwapValue(_itemToUnequip, _slotToGiveBack);
        }
        else
        {
            Debug.Log("Inventory is full. Cannot unequip this item");
        }
    }

    public int GetEquipmentTypeById(int _itemId) => (_itemId / 1000) % 10;
    #endregion

    #region Potion
    public void UsePotion(ItemInventory _item)
    {
        if (_item.itemData.type != ItemType.Potion)
            return;
        SkillManager.Instance.potion.TryConsumePotion(_item);
    }
    public void AssignPotion(ItemInventory _item)
    {
        SkillManager.Instance.potion.AssignPotion(_item);
    }
    #endregion

    #region Buff
    public void UseBuff(ItemInventory _item)
    {
        if (_item.itemData.type != ItemType.Food)
            return;
        PlayerManager.Instance.player.stats.BuffManager.StartBuff(_item.itemData);
        _item.RemoveItem();
    }
    #endregion

    #region Skill book
    public void UseSkillBook(ItemInventory _item)
    {
        if (_item.itemData.type != ItemType.SkillBook)
            return;
        int point = int.Parse(_item.itemData.properties[ItemUtilities.SKILL_POINT]);
        SkillManager.Instance.AddSkillPoint(point);
        _item.RemoveItem();
    }    
    #endregion

    #region Sorting inventory
    

    public void SortItemByItemType()
    {
        inventoryItems.Sort(ItemInventory.CompareByItemType);
    }

    public void SortItemByItemQuality()
    {
        inventoryItems.Sort(ItemInventory.CompareByItemQuality);
    }

    public void QuickSort(List<ItemSlot> listItemSlot, int low, int high, Func<ItemSlot, ItemSlot, int> Compare)
    {
        //Sort by swap value
        if (low < high)
        {
            int pi = Partition(listItemSlot, low, high, Compare);
            QuickSort(listItemSlot, low, pi - 1, Compare);
            QuickSort(listItemSlot, pi + 1, high, Compare);
        }
    }

    public int Partition(List<ItemSlot> listItemSlot, int low, int high, Func<ItemSlot, ItemSlot, int> Compare)
    {
        ItemSlot pivot = listItemSlot[high];
        int i = (low - 1);

        for (int j = low; j <= high - 1; j++)
        {
            if (Compare(listItemSlot[j], pivot) <= 0)
            {
                i++;
                ItemSlot.SwapItemSlot(listItemSlot[i], listItemSlot[j]);
            }
        }
        ItemSlot.SwapItemSlot(listItemSlot[i + 1], listItemSlot[high]);
        return (i + 1);
    }
    #endregion

    #region Item database
    public void GenerateItemDataDictionary()
    {
        itemDict[ItemData.Empty.id] = ItemData.Empty;
        foreach (ItemData item in itemDatabase.itemList)
        {
            itemDict[item.id] = item;
        }
    }

    [ContextMenu("Fill up item database")]
    public void FillUpItemDataBase()
    {
        itemDatabase.FillUpDatabase();
    }

    #endregion
    public void SaveData(ref GameData gameData)
    {
        InventorySaveData inventorySave = new InventorySaveData(inventorySize, Currency.Gold, Currency.Diamond, inventoryItems, equipedItems);
        gameData.InventoryData = inventorySave;
    }

    public void LoadData(GameData gameData)
    {
        InventorySaveData inventoryLoad = gameData.InventoryData;
        inventorySize = inventoryLoad.inventorySize;
        inventoryItems = inventoryLoad.inventoryItems.Select(item => (ItemInventory)item).ToList();
        equipedItems = new();
        for(int i=0; i< equipmentSize; i++)
        {
            equipedItems.Add(new ItemInventory());
        }
        foreach (var item in inventoryLoad.equipedItems)
        {
            EquipItem(item);
        }
        Currency = new();
        Currency.AddCurrency(inventoryLoad.gold, inventoryLoad.diamond);
    }

}
