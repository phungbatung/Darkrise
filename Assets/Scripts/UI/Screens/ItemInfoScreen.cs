using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoScreen : BlitzyUI.Screen
{
    private ItemInventory itemInventory;


    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemQuality;
    [SerializeField] private TextMeshProUGUI itemLevel;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private RectTransform rect;
    [SerializeField] private RectTransform content;
    [SerializeField] private float maxHeight;

    private List<BtnBaseItemInfo> buttonList;

    private void Awake()
    {
        
    }
    public void SetItemInfo(ItemInventory _itemInventory)
    {
        gameObject.SetActive(true);
        itemInventory = _itemInventory;
        ItemData item = _itemInventory.itemData;
        //Base info
        itemIcon.sprite = item.icon;
        itemName.text = item.name;
        itemType.text = item.type.ToString();
        itemQuality.text = item.rarity.ToString();
        //Properties, description
        string description = "";
        if (item.type == ItemType.Equipment)
        {
            string baseStat = ItemUtilities.GetBaseStatOfEquipment(item);
            Dictionary<string, string> _properties = itemInventory.equipmentProperties.GetBaseProperties();
            foreach (var property in _properties)
            {
                string[] values = property.Value.Split(new char[] { ',' });
                foreach (var value in values)
                {
                    description += $"Base {property.Key}: +{value}\n";
                }
            }
            _properties = itemInventory.equipmentProperties.GetProperties();
            foreach (var property in _properties)
            {
                string[] values = property.Value.Split(new char[] { ',' });
                foreach (var value in values)
                {
                    description += $"{property.Key} +{value}\n";
                }
            }
            description += "\n";
            for (int i=0; i<itemInventory.equipmentProperties.unlockedGemsSlot; i++)
            {
                if(itemInventory.equipmentProperties.gems[i].IsEmpty())
                {
                    ItemData gem = itemInventory.equipmentProperties.gems[i].itemData;
                    description += gem.name +"\n";
                    foreach (var _property in gem.properties)
                    {
                        description += $"+{_property.Value} {_property.Key}\n";
                    }
                }
            }   
        }
        else if (item.type == ItemType.Potion)
        {
            description += $"Instantly restores ";
            foreach(var _property in item.properties)
            {
                if (_property.Key == ItemUtilities.COOLDOWN)
                    continue;
                if(!description.EndsWith(' '))
                {
                    description += ",";
                }
                description += $"{_property.Value} {_property.Key}";
            }
            description += $"\nCooldown: {item.properties[ItemUtilities.COOLDOWN]}s\n";
            description += $"Level required: {item.level}\n";
        }
        else if (item.type == ItemType.Buff)
        {
            description += $"Effect:\n";
            foreach (var _property in item.properties)
            {
                if (_property.Key == ItemUtilities.DURATION)
                    continue;
                description += $"+{_property.Value} {_property.Key}\n";
            }
            description += $"Duration: {item.properties[ItemUtilities.DURATION]}s\n";
            description += $"\n{item.description}\n";
        }
        else if (item.type == ItemType.SkillBook)
        {
            description += $"Effect: Active skill points +{item.properties[ItemUtilities.SKILL_POINT]}\n";
            description += $"\n{item.description}\n";
        }
        else if (item.type == ItemType.MagicDust)
        {
            description += $"Effect:\n";
            foreach (var _property in item.properties)
            {
                description += $"+{_property.Value} {_property.Key}\n";
            }
            description += $"\n{item.description}\n";
        }
        else
            description = $"{item.description}\n";
        itemDescription.text = description;
        //
        CheckForActiveButton(item);
        RefreshContentSize();

    }

    private void SetBaseInfo(ItemData item)
    {
        
    }

    private void SetPropertiesInfo(ItemData item)
    {
        
    }

    private void CheckForActiveButton(ItemData item)
    {
        ItemType type = item.type;
        foreach(var button in buttonList)
        {
            if (button.CanBeActive(type))
                button.gameObject.SetActive(true);
            else
                button.gameObject.SetActive(false);
        }    
    }    

    private void RefreshContentSize()
    {
        IEnumerator Routine()
        {
            var csf = content.GetComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            yield return null;
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            yield return null;
            float height = maxHeight;
            if (content.sizeDelta.y < height)
                height = content.sizeDelta.y;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
        }
        this.StartCoroutine(Routine());
    }    

    public void EquipCurrentItem()
    {
        ItemManager.Instance.EquipItem(itemInventory);
        BlitzyUI.UIManager.Instance.QueuePop(null);
    }

    public void RemoveItem()
    {
        itemInventory.RemoveAll();
        BlitzyUI.UIManager.Instance.QueuePop(null);
    }

    public void AssignPotionToSlot()
    {
        ItemManager.Instance.AssignPotion(itemInventory);
        BlitzyUI.UIManager.Instance.QueuePop(null);
    }

    public void UsePotion()
    {
        ItemManager.Instance.UsePotion(itemInventory);
        BlitzyUI.UIManager.Instance.QueuePop(null);
    }

    public void UseBuff()
    {
        ItemManager.Instance.UseBuff(itemInventory);
        BlitzyUI.UIManager.Instance.QueuePop(null);
    }

    public void UseSkillBook()
    {
        ItemManager.Instance.UseSkillBook(itemInventory);
        BlitzyUI.UIManager.Instance.QueuePop(null);
    }

    public override void OnSetup()
    {
        rect = GetComponent<RectTransform>();
        buttonList = content.GetComponentsInChildren<BtnBaseItemInfo>().ToList();
    }

    public override void OnPush(Data data)
    {
        SetItemInfo(data.Get<ItemInventory>("ItemInventory"));
        PushFinished();
    }

    public override void OnPop()
    {
        PopFinished();
    }

    public override void OnFocus()
    {

    }

    public override void OnFocusLost()
    {

    }
}
