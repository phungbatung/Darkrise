using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionShopScreen : BlitzyUI.Screen
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

        List<ItemData> itemsData = ItemManager.Instance.GetAllItemOfType(ItemType.Potion);
        ItemShop itemShop;
        foreach (ItemData item in itemsData)
        {
            ItemInventory itemInventory = ItemManager.Instance.BuildInventoryItem(item);
            KeyValuePair<CurrencyType, int> price = new KeyValuePair<CurrencyType, int>(CurrencyType.Gold, item.SellPrice*2);
            itemShop = new ItemShop(itemInventory, price);
            Instantiate(itemShopTemplate, listItemParent).SetItem(itemShop);
        }

        PushFinished();
    }

    public override void OnSetup()
    {

    }
}
