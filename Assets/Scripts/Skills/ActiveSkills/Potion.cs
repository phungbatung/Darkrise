using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private Player player { get; set; }
    public ItemInventory itemInventory { get; private set; }
    public float cooldownTimer {  get; private set; }
    public float cooldown { get; private set; }
    private bool isCooldownCompleted { get; set; } = true;
    public Action CooldownAction { get; set; }
    public Action OnConsumePotion { get; set; }
    public Action OnAssignPotion { get; set; }
    [SerializeField] private KeyCode keyCode;

    private void Start()
    {
        player = PlayerManager.Instance.player;
    }
    private void Update()
    {
        if(!isCooldownCompleted)
        {
            cooldownTimer -= Time.deltaTime;
            CooldownAction?.Invoke();
            if (cooldownTimer <= 0)
                isCooldownCompleted = true;
        }
        if(itemInventory!=null && ItemManager.Instance!=null && ItemManager.Instance.itemDict[itemInventory.itemId].type!=ItemType.Potion)
        {
            UnassignPotion();
        }
        if (Input.GetKeyDown(keyCode))
            Consume();
    }
    public void Consume()
    {
        TryConsumePotion(itemInventory);
        if (itemInventory.amount <= 0)
            UnassignPotion();
    }
    public bool TryConsumePotion(ItemInventory _item)
    {
        if (!isCooldownCompleted)
            return false;
        if (_item == null)
            return false;
        ItemData itemData = ItemManager.Instance.itemDict[_item.itemId];
        if (itemData.type != ItemType.Potion)
            return false;

        if (itemData.TryGetProperty(ItemUtilities.HEALTH, out int _health))
        {
            player.stats.HealthIncrement(_health);
        }
        if (itemData.TryGetProperty(ItemUtilities.MANA, out int _mana))
        {
            player.stats.ManaIncreament(_mana);
        }
        cooldown = itemData.GetProperty<float>(ItemUtilities.COOLDOWN);
        cooldownTimer = cooldown;
        isCooldownCompleted = false;
        _item.RemoveItem();
        OnConsumePotion?.Invoke();
        return true;
    }

    public void AssignPotion(ItemInventory _item)
    {
        itemInventory = _item;
        OnAssignPotion?.Invoke();
    }
    public void UnassignPotion()
    {
        itemInventory = null;
        OnAssignPotion?.Invoke();
    }    
}
