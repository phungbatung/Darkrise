using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SkillManager : MonoBehaviour, ISaveManager
{
    public static SkillManager Instance { get; private set; }

    public Potion potion { get; private set; }
    public Dash dash { get; private set; }
    public BaseAttack baseAttack { get; private set; }
    public Slash slash { get; private set; }
    public HealWave healWave { get; private set; }
    public LightCut lightCut { get; private set; }
    public WolfCall wolfCall { get; private set; }

    public int skillPoint { get; set; }
    public Action OnSkillPointChange { get; set; }

    public Skill[] assignedSkills { get; private set; }
    public Action assignEvent { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        potion = GetComponent<Potion>();
        dash = GetComponent<Dash>();
        baseAttack = GetComponent<BaseAttack>();
        slash = GetComponent<Slash>();
        healWave = GetComponent<HealWave>();
        lightCut = GetComponent<LightCut>();
        wolfCall = GetComponent<WolfCall>();

        assignedSkills = new Skill[6];
        DefaulAssignmentSlot();
    }

    public void DefaulAssignmentSlot()
    {
        AssignSkillToSlot(dash, 0);
        AssignSkillToSlot(baseAttack, 1);
        AssignSkillToSlot(slash, 2);
        AssignSkillToSlot(healWave, 3);
        AssignSkillToSlot(lightCut, 4);
        AssignSkillToSlot(wolfCall, 5);
        
    }

    public void AddSkillPoint(int _point)
    {
        skillPoint += _point;
        OnSkillPointChange?.Invoke();
        //Debug.Log(skillPoint);
    }
    public bool RemoveSkillPoint(int _point)
    {
        if (skillPoint < _point)
            return false;
        skillPoint -= _point;
        OnSkillPointChange?.Invoke();
        Debug.Log(skillPoint);
        return true;
    }


    public void AssignSkillToSlot(Skill _skill, int _slotIndex)
    {
        assignedSkills[_slotIndex] = _skill;
        assignEvent?.Invoke();
  
    }

#if UNITY_EDITOR
    [ContextMenu("Create skill database")]
    public void CreateSkillDatabase()
    {
        string dataPath = "SkillData/Data";
        string iconPath = "SkillData/Icon";
        TextAsset[] listText = Resources.LoadAll<TextAsset>(dataPath);
        Sprite[] listIcon = Resources.LoadAll<Sprite>(iconPath);
        Debug.Log(listText.Length + " " + listIcon.Length);
        SkillData skillData;
        for (int i = 0; i < listText.Length; i++)
        {
            Debug.Log(i);
            skillData = CreateSkillData(i, listText[i].name, listIcon[i], listText[i].text);
            AssetDatabase.CreateAsset(skillData, $"Assets/Data/SkillData/{skillData.skillName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    public SkillData CreateSkillData(int id, string name, Sprite icon, string levelsData)
    {
        SkillData skillData = ScriptableObject.CreateInstance<SkillData>(); ;
        skillData.id = id;
        skillData.skillName = name;
        skillData.icon = icon;
        string[] lines = levelsData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        string[] keys = lines[0].Split(',');
        for (int i = 1; i < lines.Length; i++)
        {
            SkillLevelData levelData = new SkillLevelData();
            string[] values = lines[i].Split(",");
            if (values.Length != keys.Length)
                continue;
            for (int j = 0; j < values.Length; j++)
            {
                levelData.properties.Add(keys[j], values[j]);
            }
            skillData.levelsData.Add(levelData);
        }
        return skillData;
    }


    void SaveAsset(UnityEngine.Object @object)
    {
        EditorUtility.SetDirty(@object);
        AssetDatabase.SaveAssets();
    }

#endif
    public void SaveData(ref GameData gameData)
    {
        List<Skill> skills = GetComponents<Skill>().ToList();
        SerializableDictionary<int, int> skillDataSave = new SerializableDictionary<int, int>();
        foreach (var skill in skills)
        {
            skillDataSave.Add(skill.SkillData.id, skill.currentLevel);
        }

        ActiveSkillSaveData dataSave = new ActiveSkillSaveData(skillPoint, skillDataSave);
        gameData.ActiveSkillData = dataSave;
    }

    public void LoadData(GameData gameData)
    {
        List<Skill> skills = GetComponents<Skill>().ToList();
        ActiveSkillSaveData dataLoad = gameData.ActiveSkillData;
        skillPoint = dataLoad.skillPoint;
        foreach (var kvp in dataLoad.skillData)
        {
            foreach (var skill in skills)
            {
                if(skill.SkillData.id == kvp.Key)
                {
                    skill.currentLevel = kvp.Value;
                }
            }
        }
    }
}
