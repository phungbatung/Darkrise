using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDroper : MonoBehaviour
{
    [SerializeField] private ItemObject itemObjectPrefab;
    [SerializeField] private CurrencyObject currencyObjectPrefab;
    
    public void Drop()
    {
        DropItem();
        DropCurrency();
    }

    private void DropItem()
    {
        int index = Random.Range(0, ItemManager.Instance.itemDatabase.itemList.Count);

        Instantiate(itemObjectPrefab).SetUpItem(ItemManager.Instance.itemDatabase.itemList[index], transform.position);
    }

    private void DropCurrency()
    {
        int baseValue = PlayerManager.Instance.player.levels.Level;
        int gold = Random.Range(baseValue * 10, baseValue * 100);
        int diamond = Random.Range(baseValue , baseValue * 10);
        Instantiate(currencyObjectPrefab).SetValue(CurrencyType.Gold, gold, transform.position);
        Instantiate(currencyObjectPrefab).SetValue(CurrencyType.Diamond, diamond, transform.position);
    }    
}
