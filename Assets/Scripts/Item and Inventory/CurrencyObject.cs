using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CurrencyObject : MonoBehaviour
{
    private KeyValuePair<CurrencyType, int> currency;
    [SerializeField] private SerializableDictionary<CurrencyType, Sprite> icon;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetValue(CurrencyType type, int value, Vector2 dropPosition)
    {
        currency = new KeyValuePair<CurrencyType, int>(type, value);
        sr.sprite = icon[type];
        transform.position = dropPosition;
        transform.SetParent(MapManager.Instance.currentMap.Map.transform);
        rb.velocity = new Vector2(UnityEngine.Random.Range(-5.0f, 5.0f), 5);
    }

    public void PickUp()
    {
        ItemManager.Instance.Currency.AddCurrency(currency);
        gameObject.Despawn();
    }    

    

    [ContextMenu("GenerateIconDictionary")]
    public void GenerateIconDictionary()
    {
        icon.Clear();
        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
        {
            icon.Add(type, null);
        }
    }
}
