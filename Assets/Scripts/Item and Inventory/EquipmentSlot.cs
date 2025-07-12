using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : ItemSlot
{
    public EquipmentType equipmentType;

    protected override Sprite GetDefaultBackGround()
    {
        return AssetManager.Instance.GetItemSlotBackGroundImageByKey(equipmentType.ToString());
    }
}
