using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemToPick : MonoBehaviour, IPointerClickHandler
{
    private Image itemIcon;
    private TextMeshProUGUI itemName;
    private ItemObject itemObject;


    private void Awake()
    {
        itemIcon = GetComponent<Image>();
        itemName = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetUpUI(ItemObject _itemObject)
    {
        gameObject.SetActive(true);
        itemObject = _itemObject;
        itemIcon.sprite = itemObject.item.itemData.Icon;
        itemName.text = itemObject.item.itemData.Name;
        itemName.color = AssetManager.Instance.GetColorRarityByKey(itemObject.item.itemData.Rarity.ToString());
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(itemObject.PickUpItem())
            Destroy(gameObject);
    }


}
