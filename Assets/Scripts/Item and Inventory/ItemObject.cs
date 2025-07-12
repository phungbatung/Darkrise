using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    public ItemInventory item;
    public int itemId;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetUpItem(ItemData itemData, Vector3 _dropPosition)
    {
        item = ItemManager.Instance.BuildInventoryItem(itemData);
        sr.sprite = itemData.icon;
        transform.position = _dropPosition;
        rb.velocity = new Vector2(UnityEngine.Random.Range(-5.0f, 5.0f), 5);
    }

    public void PickUpItem()
    {
        if(ItemManager.Instance.TryAddItem(item))
            Destroy(gameObject);
    }

}

