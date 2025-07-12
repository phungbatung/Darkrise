using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpawnItemButton : MonoBehaviour
{
    private Button btn;
    public GameObject itemObjectPrefab;
    public ItemType itemType;
    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SpawnItemObject);
    }
    public void SpawnItemObject()
    {
        if (itemType == ItemType.None)
            SpawnRandom();
        else
            SpawnRandomOfType();
    }

    public void SpawnRandom()
    {
        int index = Random.Range(0, ItemManager.Instance.itemDatabase.itemList.Count);

        GameObject itemGameObject = Instantiate(itemObjectPrefab);
        itemGameObject.GetComponent<ItemObject>()?.SetUpItem(ItemManager.Instance.itemDatabase.itemList[index], 
                                                    PlayerManager.Instance.player.transform.position + new Vector3(0, 3, 0));
    }
    public void SpawnRandomOfType()
    {
        List<ItemData> items = new();
        foreach(ItemData item in ItemManager.Instance.itemDatabase.itemList)
        {
            if (item.type == itemType)
                items.Add(item);
        }
        if (items.Count <= 0)
            return;
        int index = Random.Range(0, items.Count);
        GameObject itemGameObject = Instantiate(itemObjectPrefab);
        itemGameObject.GetComponent<ItemObject>()?.SetUpItem(items[index], PlayerManager.Instance.player.transform.position + new Vector3(0, 3f, 0));
    }    
}
