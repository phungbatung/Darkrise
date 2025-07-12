using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private CharacterStats characterStats;


    [SerializeField] private GameObject buffViewPrefab;
    [SerializeField] private GameObject buffPresenterPrefab;
    public Transform buffHolder { get; set; }

    private Dictionary<BuffType, BuffModel> buffDict = new();
    private List<BuffModel> buffs = new();
    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        foreach (BuffModel buff in buffs)
        {
            buff.Countdown(Time.deltaTime);
        }
    }
    public void StartBuff(ItemData itemData, float duration = 0)
    {
        EndBuffOfTheSameType(itemData.id);  
        float buffDuration = duration > 0 ? duration : float.Parse(itemData.properties[ItemUtilities.DURATION]);
        BuffModel buff = new BuffModel(characterStats, itemData.id, buffDuration);

        GameObject BuffViewInstance = Instantiate(buffViewPrefab);
        BuffView buffView = BuffViewInstance.GetComponent<BuffView>();
        buffView.transform.SetParent(buffHolder);

        GameObject buffPresenterInstance = Instantiate(buffPresenterPrefab);
        BuffPresenter buffPresenter = buffPresenterInstance.GetComponent<BuffPresenter>();
        buffPresenter.Setup(buff, buffView);

        buffDict[ItemUtilities.GetBuffTypeById(itemData.id)] = buff;
        buffs.Add(buff);

        buff.EndBuffArgEvent += EndBuffOfTheSameType;
        buff.StartBuff();
    }
    public void EndBuffOfTheSameType(int id)
    {
        if (buffDict.TryGetValue(ItemUtilities.GetBuffTypeById(id), out BuffModel buff))
        {
            buff.EndBuff();
            buffs.Remove(buff);
            buffDict.Remove(ItemUtilities.GetBuffTypeById(id));
        }    

    }
}
