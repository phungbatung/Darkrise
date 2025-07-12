using System.Collections.Generic;
using UnityEngine;
using System.Linq;
# if UNITY_EDITOR
using UnityEditor;
# endif

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Data/ItemDataBase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> itemList;
  
    public void FillUpDatabase()
    {
        Dictionary<int, ItemData> itemDict = new();
        itemList.Clear(); 
        FillUpGeneralData(itemDict); //fill common properties
        FillUpEquipmentProperties(itemDict);
        FillUpPotionProperties(itemDict);
        FillUpSkillBookProperties(itemDict);
        FillUpBuffProperties(itemDict);
        FillUpMagicDustProperties(itemDict);
#if UNITY_EDITOR
        SaveAsset(this);
#endif
    }
#if UNITY_EDITOR
    void SaveAsset(Object @object)
    {
        EditorUtility.SetDirty(@object);
        AssetDatabase.SaveAssets();
    }
#endif
    private void FillUpGeneralData(Dictionary<int, ItemData> itemDataDictionary)
    {
        string[] paths = { "Rare", "Epic", "Legend" };
        List<Sprite> sprites = new();
        foreach (var path in paths)
        {
            sprites.AddRange(Resources.LoadAll<Sprite>($"ItemDataBase\\ItemIcons\\{path}"));
        }

        string itemsInfo = Resources.Load<TextAsset>("ItemDataBase\\ItemInfo").text;
        string[] listItemInfo = itemsInfo.Split(new char[] { '\n' });
        ItemData item = new();
        for (int i = 1; i < listItemInfo.Length - 1; i++)
        {
            string[] data = listItemInfo[i].Split(new char[] { ',' });
            if (data[0] != "")
            {
                item = new() 
                { 
                    id = int.Parse(data[0]),
                    type = (ItemType)(int.Parse(data[0]) / 100000),
                    rarity = (ItemRarity)((int.Parse(data[0]) / 10000) % 10),
                    name = data[1],
                    icon = sprites.Single(s => s.name == data[2]),
                    level = int.Parse(data[3]),
                    description = data[4],
                    maxSize = int.Parse(data[5]),
                    sellPrice = int.Parse(data[6]),
                };
                itemList.Add(item);
                itemDataDictionary[item.id] = item;
            }
        }
    }

    private void FillUpEquipmentProperties(Dictionary<int, ItemData> itemDataDictionary)
    {
        string equipmentsData = Resources.Load<TextAsset>("ItemDataBase\\EquipmentData").text;
        string[] listEquipmentData = equipmentsData.Split(new char[] { '\n' });
        ItemData item;
        string[] propertiesName = { "", ItemUtilities.DAMAGE, ItemUtilities.ATTACK_SPEED, ItemUtilities.ARMOR_PENETRATION, 
                                        ItemUtilities.CRITICAL_RATE, ItemUtilities.CRITICAL_DAMAGE, ItemUtilities.HEALTH, ItemUtilities.HEALTH_REGEN,
                                        ItemUtilities.ARMOR, ItemUtilities.MANA, ItemUtilities.MANA_REGEN, ItemUtilities.MOVE_SPEED};
        for (int i = 1; i < listEquipmentData.Length; i++)
        {
            string[] data = listEquipmentData[i].Split(new char[] { ',', '\r' });
            if (int.TryParse(data[0], out int _id))
            {
                item = itemDataDictionary[_id];
                for (int j = 1; j < data.Length; j++)
                {
                    if (data[j] != "0" && data[j].Length!=0)
                        item.properties[propertiesName[j]] = data[j];
                }
            }
        }
    }

    private void FillUpPotionProperties(Dictionary<int, ItemData> itemDataDictionary)
    {
        string potionsData = Resources.Load<TextAsset>("ItemDataBase\\PotionData").text;
        string[] listPotionData = potionsData.Split(new char[] { '\n' });
        ItemData item;
        string[] propertiesName = { "", ItemUtilities.HEALTH, ItemUtilities.MANA, ItemUtilities.COOLDOWN};
        for (int i=1; i<listPotionData.Length; i++)
        {
            string[] data = listPotionData[i].Split(new char[] { ',', '\r' });
            if (int.TryParse(data[0], out int _id))
            {
                item = itemDataDictionary[_id];
                for (int j = 1; j < data.Length; j++)
                {
                    if (data[j] != "0" && data[j].Length != 0)
                        item.properties[propertiesName[j]] = data[j];
                }
            }
        }
    }

    private void FillUpSkillBookProperties(Dictionary<int, ItemData> itemDataDictionary)
    {
        string skillBooksData = Resources.Load<TextAsset>("ItemDataBase\\SkillBookData").text;
        string[] listSkillBookData = skillBooksData.Split(new char[] { '\n' });
        ItemData item;
        
        for (int i = 1; i < listSkillBookData.Length; i++)
        {
            string[] data = listSkillBookData[i].Split(new char[] { ',', '\r' });
            if (data[0] != "")
            {
                item = itemDataDictionary[int.Parse(data[0])];
                if (data[1] != "0")
                    item.properties[ItemUtilities.SKILL_POINT] = data[1];
            }
        }
    }

    private void FillUpBuffProperties(Dictionary<int, ItemData> itemDataDictionary)
    {
        string buffsData = Resources.Load<TextAsset>("ItemDataBase\\BuffData").text;
        string[] listBuffData = buffsData.Split(new char[] { '\n' });
        ItemData item;
        string[] propertiesName = { "", ItemUtilities.DAMAGE, ItemUtilities.HEALTH, ItemUtilities.DURATION };
        for (int i = 1; i < listBuffData.Length; i++)
        {
            string[] data = listBuffData[i].Split(new char[] { ',', '\r' });
            if (int.TryParse(data[0], out int _id))
            {
                item = itemDataDictionary[_id];
                for (int j = 1; j < data.Length; j++)
                {
                    if (data[j] != "0" && data[j].Length != 0)
                        item.properties[propertiesName[j]] = data[j];
                }
            }
        }
    }

    private void FillUpMagicDustProperties(Dictionary<int, ItemData> itemDataDictionary) 
    {
        string magicDustsData = Resources.Load<TextAsset>("ItemDataBase\\MagicDustData").text;
        string[] listMagicDustData = magicDustsData.Split(new char[] { '\n' });
        ItemData item;
        string[] propertiesName = { "", ItemUtilities.DAMAGE, ItemUtilities.HEALTH };
        for (int i = 1; i < listMagicDustData.Length; i++)
        {
            string[] data = listMagicDustData[i].Split(new char[] { ',', '\r' });
            if (int.TryParse(data[0], out int _id))
            {
                item = itemDataDictionary[_id];
                for (int j = 1; j < data.Length; j++)
                {
                    if (data[j] != "0" && data[j].Length != 0)
                        item.properties[propertiesName[j]] = data[j];
                }
            }

        }
    }

}
