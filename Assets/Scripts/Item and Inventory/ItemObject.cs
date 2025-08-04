using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private SpriteRenderer bg;
    private Rigidbody2D rb;
    public ItemInventory item;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetUpItem(ItemData itemData, Vector3 _dropPosition)
    {
        item = ItemManager.Instance.BuildInventoryItem(itemData);
        icon.sprite = itemData.Icon;
        bg.color = AssetManager.Instance.GetColorRarityByKey(itemData.Rarity.ToString());
        transform.position = _dropPosition;
        rb.velocity = new Vector2(UnityEngine.Random.Range(-5.0f, 5.0f), 5);
    }

    public bool PickUpItem()
    {
        if (ItemManager.Instance.TryAddItem(item))
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

}

