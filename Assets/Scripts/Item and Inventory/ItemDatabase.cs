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
        itemList.Clear();
        foreach (ItemType itemType in System.Enum.GetValues(typeof(ItemType)))
        {
            if (itemType == ItemType.None)
                continue;
            Debug.Log($"Fill up item data of type: {itemType}");
            string dataText = Resources.Load<TextAsset>($"ItemDataBase\\{itemType}").text;
            string[] lines = dataText.Split('\n');
            string[][] cells = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                cells[i] = lines[i].Split(new char[] { ',', '\r' });
            }
            Debug.Log($"Column count: {lines.Length}. Row count: {cells[0].Length}");
            ItemData newItemData;
            for (int i = 1; i < lines.Length; i++)
            {
                try
                {
                    newItemData = new();
                    newItemData.Type = itemType;
                    newItemData.Id = (int)itemType * 1000 + int.Parse(cells[i][0]);
                    newItemData.Name = cells[i][1];
                    newItemData.Icon = Resources.Load<Sprite>($"ItemDataBase\\ItemIcons\\Item\\{cells[i][2]}");
                    newItemData.Level = int.Parse(cells[i][3]);
                    newItemData.SubType = int.Parse(cells[i][4]);
                    newItemData.Rarity = (ItemRarity)int.Parse(cells[i][5]);
                    newItemData.Description = cells[i][6];
                    newItemData.MaxSize = int.Parse(cells[i][7]);
                    for (int j = 8; j < cells[0].Length; j++)
                    {
                        if (cells[i][j] != "0" && cells[i][j] != "")
                        {
                            newItemData.Properties[cells[0][j]] = cells[i][j];
                        }
                    }
                    itemList.Add(newItemData);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Loi khi import database: {e}");
                }
            }
        }
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
}