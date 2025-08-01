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

    public int id;
    public string name;
    public Sprite icon;
    public int level;
    public ItemType type;
    public int subType;
    public ItemRarity rarity;
    public string description;
    public int maxSize;
    public int sellPrice;
    public SerializableDictionary<string, string> properties = new SerializableDictionary<string, string>();
    public EquipmentType EquipmentType => (EquipmentType)subType;
    public BuffType BuffType => (BuffType)subType;
    public T GetProperty<T>(string key)
    {
        if (properties.TryGetValue(key, out string value))
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
        if (properties.TryGetValue(key, out string strValue))
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

    private static ItemData _emptyItem = new ItemData()
    {
        id = -1,
        name = string.Empty,
        icon = default,
        type = ItemType.None,
        subType = 0,
        rarity = ItemRarity.Common,
        description = string.Empty,
        maxSize = 0,
        sellPrice = 0,
    };
    public static ItemData Empty { get => _emptyItem; }
}
