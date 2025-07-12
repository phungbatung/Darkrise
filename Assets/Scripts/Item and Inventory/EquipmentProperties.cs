using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class EquipmentProperties
{
    public Dictionary<string, string> baseProperties { get; private set; }
    public Dictionary<string, string> properties {  get; private set; }

    public int unlockedGemsSlot { get; private set; }
    private int gemSlotsLimit { get; set; } = 3;
    public ItemInventory[] gems { get; private set; }
    private int unlockPrice { get; } = 6789;
    private int unlockPriceIncreaseRatePerSlot { get; } = 3;

    public int enhanceLevel { get; private set; }
    private int maxEnhanceLevel { get; } = 20;
    private float propertiesIncreaseRatePerLevel { get; } = 1.05f;
    private int upgradePrice { get; } = 64;
    private float priceIncreaseRatePerLevel { get; } = 1.5f;

    public EquipmentProperties(Dictionary<string, string> _baseProperties, Dictionary<string, string> _properties)
    {
        baseProperties = _baseProperties;
        properties = _properties;
        unlockedGemsSlot = 0;
        gems = new ItemInventory[gemSlotsLimit];
        for (int i =0; i<gems.Length; i++)
        {
            gems[i] = new ItemInventory();
        }
    }

    public EquipmentProperties(SerializableDictionary<string, string> _baseProperties, 
        SerializableDictionary<string, string> _properties, int _unlockedGemsSlot, ItemInventory[] _gems, int _enhanceLevel)
    {
        baseProperties = new();
        foreach (var kvp in _baseProperties)
        {
            this.baseProperties[kvp.Key] = kvp.Value;
        }
        properties = new();
        foreach (var kvp in _properties)
        {
            this.properties[kvp.Key] = kvp.Value;
        }
        unlockedGemsSlot = _unlockedGemsSlot;
        gems = _gems;
        enhanceLevel = _enhanceLevel;
    }


    //Include all properties, use to apply to stats
    public Dictionary<string, string> GetAllProperties()
    {
        Dictionary<string, string> _properties = GetEquipmentProperties();
        //add gem properties
        for (int i = 0; i < unlockedGemsSlot; i++)
        {
            if (gems[i] == null)
                continue;
            ItemData item = gems[i].itemData;
            foreach (KeyValuePair<string, string> kvp in item.properties)
            {
                
                if (_properties.ContainsKey(kvp.Key))
                {
                    _properties[kvp.Key] += $",{kvp.Value}";
                }
                else
                {
                    _properties[kvp.Key] = kvp.Value;
                }
            }
        }
        return _properties;
    }
    //Include base properties and properties, exclude gem properties
    public Dictionary<string, string> GetEquipmentProperties()
    {
        Dictionary<string, string> _properties = new();
        //add base properties
        foreach (KeyValuePair<string, string> kvp in baseProperties)
        {
            string[] values = kvp.Value.Split(',');
            foreach (string value in values)
            {
                if (_properties.ContainsKey(kvp.Key))
                {
                    _properties[kvp.Key] += $",{(int)(float.Parse(value) * GetEnhanceValue())}";
                }
                else
                {
                    _properties[kvp.Key] = ((int)(float.Parse(value) * GetEnhanceValue())).ToString();
                }
            }
        }
        //add properties
        foreach (KeyValuePair<string, string> kvp in properties)
        {
            string[] values = kvp.Value.Split(',');
            foreach (string value in values)
            {
                if (_properties.ContainsKey(kvp.Key))
                {
                    _properties[kvp.Key] += $",{(int)(float.Parse(value) * GetEnhanceValue())}";
                }
                else
                {
                    _properties[kvp.Key] = ((int)(float.Parse(value) * GetEnhanceValue())).ToString();
                }
            }
        }

        return _properties;
    }
    public Dictionary<string, string> GetBaseProperties()
    {
        Dictionary<string, string> _properties = new();
        foreach (KeyValuePair<string, string> kvp in baseProperties)
        {
            string[] values = kvp.Value.Split(',');
            foreach (string value in values)
            {
                if (_properties.ContainsKey(kvp.Key))
                {
                    _properties[kvp.Key] += $",{(int)(float.Parse(value) * GetEnhanceValue())}";
                }
                else
                {
                    _properties[kvp.Key] = ((int)(float.Parse(value) * GetEnhanceValue())).ToString();
                }
            }
        }
        return _properties;
    }
    public Dictionary<string, string> GetProperties()
    {
        Dictionary<string, string> _properties = new();
        foreach (KeyValuePair<string, string> kvp in properties)
        {
            string[] values = kvp.Value.Split(',');
            foreach (string value in values)
            {
                if (_properties.ContainsKey(kvp.Key))
                {
                    _properties[kvp.Key] += $",{(int)(float.Parse(value) * GetEnhanceValue())}";
                }
                else
                {
                    _properties[kvp.Key] = ((int)(float.Parse(value) * GetEnhanceValue())).ToString();
                }
            }
        }
        return _properties;
    }
    //Include gem properties
    public Dictionary<string, string> GetGemsProperties()
    {
        Dictionary<string, string> _properties = new();
        for (int i = 0; i < unlockedGemsSlot; i++)
        {
            if (gems[i].IsEmpty())
                continue;
            ItemData item = gems[i].itemData;
            foreach (KeyValuePair<string, string> kvp in item.properties)
            {
                if (_properties.ContainsKey(kvp.Key))
                {
                    _properties[kvp.Key] += $",{kvp.Value}";
                }
                else
                {
                    _properties[kvp.Key] = kvp.Value;
                }
            }
        }
        return _properties;
    }

    #region Enhance
    public bool TryUpgrade()
    {
        if (enhanceLevel >=maxEnhanceLevel)
        {
            return false;
        }
        if (ItemManager.Instance.Currency.TrySubtractCurrency(GetUpgradePrice()))
        {
            enhanceLevel++;
            return true;
        }
        return false;
    }

    public bool IsMaxEnhanceLevel() => enhanceLevel >= maxEnhanceLevel;

    public void Downgrade()
    {
        if (enhanceLevel <=0)
        {
            return;
        }
        enhanceLevel--;
    }

    public float GetEnhanceValue() => Mathf.Pow(propertiesIncreaseRatePerLevel, enhanceLevel);

    public float GetEnhanceValueAtLevel(int _level) => Mathf.Pow(propertiesIncreaseRatePerLevel, _level);

    public string GetPropertiesChangeWhenUpgrade()
    {
        string res = "";
        foreach (KeyValuePair<string, string> kvp in baseProperties)
        {
            Debug.Log(kvp);
            string[] values = kvp.Value.Split(',');
            foreach (string value in values)
            {
                res += $"{kvp.Key}: {(int)(float.Parse(value) * GetEnhanceValue())}->{(int)(float.Parse(value) * GetEnhanceValueAtLevel(enhanceLevel+1))}\n";
            }
        }
        foreach (KeyValuePair<string, string> kvp in properties)
        {
            string[] values = kvp.Value.Split(',');
            foreach (string value in values)
            {
                res += $"{kvp.Key}: {(int)(float.Parse(value) * GetEnhanceValue())}->{(int)(float.Parse(value) * GetEnhanceValueAtLevel(enhanceLevel + 1))}\n";
            }
        }
        return res;
        //return ((int)(float.Parse(properties[_property]) * GetEnhanceValue())).ToString();
    }

    public KeyValuePair<CurrencyType, int> GetUpgradePrice()
    {
        return new(CurrencyType.Diamond, (int)(upgradePrice * Mathf.Pow(priceIncreaseRatePerLevel, enhanceLevel))); 
    }
    #endregion

    #region Gem
    public bool TryUnlockGemSlot()
    {
        if (unlockedGemsSlot >= gemSlotsLimit)
            return false;
        if (ItemManager.Instance.Currency.TrySubtractCurrency(GetUnlockGemSlotPrice()))
        {
            unlockedGemsSlot += 1;
            return true;
        }
        return false;
    }
    public KeyValuePair<CurrencyType, int> GetUnlockGemSlotPrice()
    {
        return new KeyValuePair<CurrencyType, int>(CurrencyType.Gold, (int)(unlockPrice * Mathf.Pow(unlockPriceIncreaseRatePerSlot, unlockedGemsSlot)));
    }

    public bool TryPutGemToSlot(int slotIndex, ItemInventory item)
    {
        if (slotIndex>=unlockedGemsSlot || item.IsEmpty() || item.itemData.type != ItemType.MagicDust)
            return false;
        gems[slotIndex] = item;
        item.RemoveItem();
        return true;
    }

    public bool TryRemoveGemFromSlot(int slotIndex)
    {
        if (ItemManager.Instance.TryAddItem(gems[slotIndex]))
        {
            gems[slotIndex] = new ItemInventory();
            return true;
        }

        return false;      
    }
    #endregion

}
