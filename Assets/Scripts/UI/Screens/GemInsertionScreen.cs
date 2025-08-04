using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemInsertionScreen : BlitzyUI.Screen
{
    [SerializeField] private EquipmentWorkInventoryUI inventoryUI;
    [SerializeField] private EquipmentForGemWorkSlot equipmentSlot;
    [SerializeField] private GemsSlot gemsSlot;
    [SerializeField] private TextMeshProUGUI guideText;

    
    public override void OnFocus()
    {
        inventoryUI.UpdateItemSlot();
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
        inventoryUI.SwitchToFirstPage();
        PushFinished();
    }

    public override void OnSetup()
    {
        equipmentSlot.OnDropAction += EquipmentSlotOnDropAction;
        equipmentSlot.OnPointerDownAction += EquipmentSlotOnPoiterDownAction;
    }

    public void EquipmentSlotOnDropAction(ItemInventory _itemInventory)
    {
        inventoryUI.RemoveOnWorkItem();
        inventoryUI.SetItemToWorkOnGem(_itemInventory, EquipmentSlotOnPoiterDownAction);
        guideText.gameObject.SetActive(false);
        gemsSlot.gameObject.SetActive(true);
        gemsSlot.SetupGemsSlot(_itemInventory);
    }
    
    public void EquipmentSlotOnPoiterDownAction()
    {
        equipmentSlot.SetEmtyItem();
        inventoryUI.RemoveOnWorkItem();
        guideText.gameObject.SetActive(true);
        gemsSlot.gameObject.SetActive(false);
    }
    
    public void UpdateInventory()
    {
        inventoryUI.UpdateItemSlot();
    }
}
