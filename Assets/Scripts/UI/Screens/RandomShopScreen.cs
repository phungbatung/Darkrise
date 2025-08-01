using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShopScreen : BlitzyUI.Screen
{
    [SerializeField] private Transform listItemParent;
    [SerializeField] private UI_ItemShop itemShopTemplate;
    public override void OnFocus()
    {

    }

    public override void OnFocusLost()
    {

    }

    public override void OnPop()
    {
        PopFinished();
    }

    public override void OnPush(Data data)
    {
        for (int i = 0; i < listItemParent.childCount; i++)
            Destroy(listItemParent.GetChild(i).gameObject);

        List<ItemData> itemsData = ItemManager.Instance.GetAllItemOfType(new ItemType[] { ItemType.Equipment, 
                                    ItemType.Food, ItemType.Gem, ItemType.Material, ItemType.SkillBook });

        ItemShop itemShop;
        foreach (ItemData item in itemsData)
        {
            ItemInventory itemInventory = ItemManager.Instance.BuildInventoryItem(item);
            KeyValuePair<CurrencyType, int> price = new KeyValuePair<CurrencyType, int>(CurrencyType.Gold, item.sellPrice * 2);
            itemShop = new ItemShop(itemInventory, price);
            Instantiate(itemShopTemplate, listItemParent).SetItem(itemShop);
        }

        PushFinished();
    }

    public override void OnSetup()
    {

    }
}
