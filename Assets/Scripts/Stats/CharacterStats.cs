using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour, IDamageable, IAttacker
{
    public Stat damage;
    public Stat attackSpeed;
    public Stat armorPenetration;
    public Stat criticalRate;
    public Stat criticalDamage;

    public Stat maxHealth;
    public Stat healthRegen;
    public Stat armor;

    public Stat maxMana;
    public Stat manaRegen;
    public Stat moveSpeed;

    public int currentHealth;
    public int currentMana;

    public bool isImmortal;

    //To get Stat by key of item property quickly
    private Dictionary<string, Stat> getStatByName = new();

    public Action OnHealthChanged { get; set; }
    public Action OnManaChanged { get; set; }
    public Action OnCharacterDie { get; set; }
    public Action OnCharacterHit { get; set; }


    public BuffManager BuffManager { get; private set; }


    protected virtual void Awake()
    {
        InitGetStatByNameDict();
        currentHealth = maxHealth.GetValue();
        currentMana = maxMana.GetValue();
        BuffManager = GetComponent<BuffManager>();
    }


    private void InitGetStatByNameDict()
    {
        getStatByName[ItemUtilities.DAMAGE] = damage;
        getStatByName[ItemUtilities.ATTACK_SPEED] = attackSpeed;
        getStatByName[ItemUtilities.ARMOR_PENETRATION] = armorPenetration;
        getStatByName[ItemUtilities.CRITICAL_RATE] = criticalRate;
        getStatByName[ItemUtilities.CRITICAL_DAMAGE] = criticalDamage;
        getStatByName[ItemUtilities.HEALTH] = maxHealth;
        getStatByName[ItemUtilities.HEALTH_REGEN] = healthRegen;
        getStatByName[ItemUtilities.ARMOR] = armor;
        getStatByName[ItemUtilities.MANA] = maxMana;
        getStatByName[ItemUtilities.MANA_REGEN] = manaRegen;
        getStatByName[ItemUtilities.MOVE_SPEED] = moveSpeed;
    }


    public void AddModifier(Dictionary<string, string> _properties)
    {
        foreach (KeyValuePair<string, string> property in _properties)
        {
            if (getStatByName.TryGetValue(property.Key, out Stat stat))
            {
                string[] values = property.Value.Split(',');
                foreach (string value in values)
                {
                        stat.AddModifier(int.Parse(value));
                }
            }
            else
            {
                Debug.Log($"'{property.Key}' is not a valid stat and will be ignored.");
            }
        }
    }

    public void RemoveModifier(Dictionary<string, string> _properties)
    {
        foreach (KeyValuePair<string, string> property in _properties)
        {
            if (getStatByName.TryGetValue(property.Key, out Stat stat))
            {
                string[] values = property.Value.Split(',');
                foreach (string value in values)
                {
                    stat.RemoveModifier(int.Parse(value));
                }
            }
            else
            {
                Debug.LogError($"Does not contain stat name: {property.Key}");
            }
        }
    }


    public void TakeDamage(int _damage = 0, int _critRate = 0, int _critDamage = 0, int _armorPenetration = 0)
    {
        if (isImmortal)
            return;
        int finalDamage=_damage;
        //calculate critical
        bool critical = UnityEngine.Random.Range(0, 100) <= _critRate;
        if (critical)
            finalDamage = (int)(finalDamage * (float)(_critDamage)/100);
        //apply armor
        int finalArmor = armor.GetValue() >  _armorPenetration ? armor.GetValue() - _armorPenetration : 0;
        finalDamage -= finalArmor;
        OnCharacterHit?.Invoke();
        HealthIncrement(-finalDamage);
        if (currentHealth <= 0)
            OnCharacterDie?.Invoke();
    }

    public void DoDamage(IDamageable target, float damagePercentage = 100f)
    {
        target.TakeDamage((int)(damage.GetValue() * (damagePercentage/100)), criticalRate.GetValue(), criticalDamage.GetValue(), armorPenetration.GetValue());
    }

    public void OnAttackDealt(IDamageable target)
    {

    }    


    public void HealthIncrement(int _health = 0, int _healthPercentage = 0)
    {
        currentHealth += _health;
        currentHealth += (int)Mathf.Floor(maxHealth.GetValue() * _healthPercentage * 1f / 100f);
        if (currentHealth > maxHealth.GetValue())
            currentHealth = maxHealth.GetValue();
        else if (currentHealth < 0)
            currentHealth = 0;
        OnHealthChanged?.Invoke();
    }
    
    public virtual void ManaIncreament(int _mana = 0, int _manaPercentage = 0)
    {
        currentMana += _mana;
        currentMana +=  (int)Mathf.Floor(maxHealth.GetValue() * _manaPercentage * 1f / 100);
        if (currentMana > maxMana.GetValue())
            currentMana = maxMana.GetValue();
        else if (currentMana < 0)
            currentMana = 0;
        OnManaChanged?.Invoke();
    }

    public void Revive()
    {
        HealthIncrement(0, 100);
        ManaIncreament(0, 100);
    }    
}
