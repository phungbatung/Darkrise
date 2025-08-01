using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;


public class ItemUtilities
{
    public const string DAMAGE = "Attack";
    public const string ATTACK_SPEED = "AttackSpeed";
    public const string ARMOR_PENETRATION = "ArmorPenetration";
    public const string CRITICAL_RATE = "CriticalRate";
    public const string CRITICAL_DAMAGE = "CriticalDamage";


    public const string HEALTH = "Health";
    public const string HEALTH_REGEN = "HealthRegen";
    public const string ARMOR = "Armor";

    public const string MANA = "Mana";
    public const string MANA_REGEN = "ManaRegen";
    public const string MOVE_SPEED = "MoveSpeed";

    public const string COOLDOWN = "Cooldown";
    public const string DURATION = "Duration";

    public const string HEALTH_BUFF = "HealthBuff";
    public const string DAMAGE_BUFF = "DamageBuff";

    public const string SKILL_POINT = "SkillPoint";

    public static string GetBaseStatOfEquipment(ItemData _itemData)
    {
        EquipmentType equipmentType = _itemData.EquipmentType;
        if (equipmentType == EquipmentType.Sword) return DAMAGE;
        else if (equipmentType == EquipmentType.Boots) return MOVE_SPEED;
        else return ARMOR;
    }

    public static BuffType GetBuffTypeById(int _itemId)
    {
        return (BuffType)(_itemId / 1000 % 10);
    }

    public static Dictionary<string, string> GetBaseProperties(ItemData _itemData)
    {
        Dictionary<string, string> dict = new();
        string baseProperties = GetBaseStatOfEquipment(_itemData);
        dict.Add(baseProperties, _itemData.properties[baseProperties]);
        return dict;
    }
}
