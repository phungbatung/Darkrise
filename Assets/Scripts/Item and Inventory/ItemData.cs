using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None=0,
    Equipment=1,
    Potion=2,
    SkillBook=3,
    Food = 4,
    Gem=5,
    Material=6
}
public enum ItemRarity
{
    Common=0,
    Uncommon=1,
    Rare=2,
    Epic=3,
    Legend=4
}

public enum EquipmentType
{
    Sword = 0,
    Shield = 1,
    Gauntlet = 2,
    Boots = 3,
    ChestPlate = 4,
    Pants = 5,
    Helmet = 6,
    Ring = 7
}
public enum BuffType
{
    Attack = 0,
    Health = 1,
    MoveSpeed = 2,
    ArmorPenetration = 3,
    Armor = 4,
    AttackSpeed = 5
}
[System.Serializable]
public class ItemData
{

    public int Id;
    public string Name;
    public Sprite Icon;
    public int Level;
    public ItemType Type;
    public int SubType;
    public ItemRarity Rarity;
    public string Description;
    public int MaxSize;
    public int SellPrice;
    public SerializableDictionary<string, string> Properties = new SerializableDictionary<string, string>();

    public ItemData()
    {

    }
    public ItemData(int id, string name, Sprite icon, int level, ItemType type, int subType, ItemRarity rarity, string description, int maxSize, int sellPrice, SerializableDictionary<string, string> properties)
    {
        Id = id;
        Name = name;
        Icon = icon;
        Level = level;
        Type = type;
        SubType = subType;
        Rarity = rarity;
        Description = description;
        MaxSize = maxSize;
        SellPrice = sellPrice;
        Properties = properties;
    }

    public EquipmentType EquipmentType => (EquipmentType)SubType;
    public BuffType BuffType => (BuffType)SubType;
    public T GetProperty<T>(string key)
    {
        if (Properties.TryGetValue(key, out string value))
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Cannot change type '{value}' to {typeof(T)}");
            }
        }
        throw new KeyNotFoundException($"Key '{key}' not found in dictionary.");
    }
    public bool TryGetProperty<T>(string key, out T value)
    {
        if (Properties.TryGetValue(key, out string strValue))
        {
            try
            {
                value = (T)Convert.ChangeType(strValue, typeof(T));
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        value = default;
        return false;
    }

    private static readonly ItemData _emptyItem = new ItemData(
        -1,
        string.Empty,
        default,
        0,
        ItemType.None,
        0,
        ItemRarity.Common,
        string.Empty,
        0,
        0,
        new SerializableDictionary<string, string>()
    );
    

    public static ItemData Empty
    {
        get
        {
            return _emptyItem;
        }
    }
}
