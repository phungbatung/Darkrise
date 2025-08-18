using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStats : CharacterStats
{
    [SerializeField] private EnemyBaseStatsData baseData;
    public static readonly float growthRate = 1.08f;

    protected override void Awake()
    {
        ApplyLevelGrowth();
        base.Awake();
    }
    public void ApplyLevelGrowth()
    {
        CharacterLevel characterLevel = GetComponent<CharacterLevel>();
        int level = characterLevel!=null? characterLevel.Level : 0;
        float growthValue = Mathf.Pow(growthRate, level);

        damage.AddModifier((int)(baseData.damage * growthValue));
        armorPenetration.AddModifier((int)(baseData.armorPenetration * growthValue));
        criticalRate.AddModifier((int)(baseData.criticalRate * growthValue));
        criticalDamage.AddModifier((int)(baseData.criticalDamage * growthValue));
        maxHealth.AddModifier((int)(baseData.maxHealth * growthValue));
        armor.AddModifier((int)(baseData.armor * growthValue));
    }    
}
