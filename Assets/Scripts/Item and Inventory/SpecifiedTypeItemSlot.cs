using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpecifiedTypeItemSlot : ItemSlot
{
    [SerializeField] private ItemType typeSpecified;


    public override void OnDrop(PointerEventData eventData)
    {
        ItemSlot toDropSlot = eventData.pointerDrag.GetComponent<ItemSlot>();
        if (toDropSlot.itemInventory.itemData.Type != typeSpecified)
            return;
        ItemManager inventory = ItemManager.Instance;

        //Case: slot to drop is equipment slot
        if (ItemManager.Instance.equipedItems.Contains(itemInventory))
        {
            //Two equipped item are not the same type
            if (inventory.equipedItems.Contains(toDropSlot.itemInventory))
                return;
            //Cannot drop non-equipment to equipment slot
            if (toDropSlot.itemInventory.itemData.Type != ItemType.Equipment)
                return;
            //Cannot drop equipment of different type in this equipment slot
            if (inventory.GetEquipmentTypeById(toDropSlot.itemInventory.itemData.Id) != inventory.equipedItems.IndexOf(itemInventory))
                return;
            //Equip
            inventory.EquipItem(toDropSlot.itemInventory);
            return;
        }

        //Case: slot to drop is inventory slot
        //Check if to drop slot is from equipment slot
        if (ItemManager.Instance.equipedItems.Contains(toDropSlot.itemInventory))
        {
            //if slot to drop is empty then move item from equipment slot to this slot
            if (itemInventory.IsEmpty())
            {
                inventory.UnequipItem(toDropSlot.itemInventory, itemInventory);
                return;
            }
            //Cannot swap item from equipment slot to none equipment
            if (itemInventory.itemData.Type != ItemType.Equipment)
                return;
            //Cannot swap item from equipment slot to another equipment of different type
            if (inventory.GetEquipmentTypeById(itemInventory.itemData.Id) != inventory.GetEquipmentTypeById(toDropSlot.itemInventory.itemData.Id))
                return;
            //Equip this item if it is equipment of the same type
            inventory.EquipItem(itemInventory);
            return;
        }
        //Swap two item from inventory
        SwapItemSlot(this, toDropSlot);
    }
}
