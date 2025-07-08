using System;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected Player player { get; set; }
    [SerializeField] private SkillData skillData;
    public SkillData SkillData { get { return skillData; } }
    public int currentLevel { get; set;}
    public  float cooldownTimer {  get; protected set; }
    protected bool isCooldownCompleted { get; set; }
    public Action cooldownEvent { get; set; }
    public bool isPressed { get; set; }
    [SerializeField] protected KeyCode keyCode;
    
    protected virtual void Awake()
    {
        //FillData();
        currentLevel = 0;
    }
    protected virtual void Start()
    {
        player = PlayerManager.Instance.player;
    }

    protected virtual void Update()
    {
        if (!isCooldownCompleted)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownEvent?.Invoke();
            if (cooldownTimer <= 0)
                isCooldownCompleted = true;
        }
    }

    public virtual void Called()
    {
        cooldownTimer = SkillData.levelsData[currentLevel].GetProperty<float>(SkillLevelData.Key.COOLDOWN);
        isCooldownCompleted = false;
        player.stats.ManaIncreament(-SkillData.levelsData[currentLevel].GetProperty<int>(SkillLevelData.Key.MANA_COST));
    }

    public virtual bool CanBeUse()
    {
        return (isPressed||Input.GetKey(keyCode)) && cooldownTimer <= 0 && player.stats.currentMana >= SkillData.levelsData[currentLevel].GetProperty<int>(SkillLevelData.Key.MANA_COST);
    }

    public int GetPointToUpgradeNextLevel()
    {
        return (int)Mathf.Pow(5, currentLevel + 1);
    }
    public virtual void Upgrade()
    {
        int pointToUpgrade = GetPointToUpgradeNextLevel();
        if (currentLevel < SkillData.levelsData.Count - 1 && pointToUpgrade <= SkillManager.Instance.skillPoint)
        {
            SkillManager.Instance.RemoveSkillPoint(pointToUpgrade);
            currentLevel++;
        }
    }


    public virtual string GetUnlockedLevelDescription()
    {
        string desc = "";
        for(int level=0; level<=currentLevel; level++)
        {
            desc += $"LV{SkillData.levelsData[level].GetProperty<string>(SkillLevelData.Key.LEVEL)}: {SkillData.levelsData[level].GetProperty<string>(SkillLevelData.Key.DESCRIPTION)}\n";
        }
        return desc;
    }
    public virtual string GetLockedLevelDescription()
    {
        string desc = "";
        for (int level = currentLevel+1; level < SkillData.levelsData.Count; level++)
        {
            desc += $"LV{SkillData.levelsData[level].GetProperty<string>(SkillLevelData.Key.LEVEL)}: {SkillData.levelsData[level].GetProperty<string>(SkillLevelData.Key.DESCRIPTION)}\n";
                
        }
        return desc;
    }
    public abstract string GetDescription();
    //public abstract void FillData();

}
