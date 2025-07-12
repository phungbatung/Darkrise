using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionSlot : MonoBehaviour, IPointerDownHandler
{
    private Potion potion;
    [SerializeField] private Image image;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TextMeshProUGUI textCoolDown;
    [SerializeField] private TextMeshProUGUI amount;
    public Action OnPress { get; set; }

    private void Start()
    {
        potion = SkillManager.Instance.potion;
        OnPress += SkillManager.Instance.potion.Consume;
        SkillManager.Instance.potion.CooldownAction += DoCooldown;
        SkillManager.Instance.potion.OnConsumePotion += SetAmount;
        SkillManager.Instance.potion.OnAssignPotion +=AssignPotion;

        AssignPotion();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress?.Invoke();
    }

    public void DoCooldown()
    {
        float cooldownTimer = potion.cooldownTimer;
        float cooldown = potion.cooldown;
        if (cooldownTimer <= 0)
        {
            cooldownImage.fillAmount = 0;
            textCoolDown.text = "";
            return;
        }
        cooldownImage.fillAmount = cooldownTimer<=0?0:cooldownTimer / cooldown;
        textCoolDown.text = ((int)cooldownTimer).ToString();
    }

    public void AssignPotion()
    {
        if(potion.itemInventory == null|| potion.itemInventory.IsEmpty())
        {
            image.sprite = null;
            amount.text = "";
            return;
        }

        ItemData item = potion.itemInventory.itemData;
        if (item.type != ItemType.Potion)
        {
            image.sprite = null;
            amount.text = "";
            return;
        }
        image.sprite = item.icon;
        amount.text = potion.itemInventory.amount.ToString();
    }

    public void SetAmount()
    {
        amount.text = potion.itemInventory!=null? potion.itemInventory.amount.ToString():"";
    }
}
