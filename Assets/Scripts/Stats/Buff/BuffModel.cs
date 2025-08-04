using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class BuffModel
{
    private CharacterStats characterStats;
    public int itemId { get; private set; }
    public float timeLeft { get; private set; }
    public Action<float> CountdownEvent { get; set; }
    public Action<int> EndBuffArgEvent { get; set; }
    public Action EndBuffNotArgEvent { get; set; }
    public BuffModel(CharacterStats _characterStats, int _itemId,  float _timeLeft)
    {
        characterStats = _characterStats;
        itemId = _itemId;
        timeLeft = _timeLeft;
        EndBuffNotArgEvent += EndBuff;
    }

    public void Countdown(float _deltaTime)
    {
        timeLeft -= _deltaTime;
        CountdownEvent?.Invoke(timeLeft);
        CheckForEndBuff();
    }

    public void CheckForEndBuff()
    {
        if (timeLeft <= 0)
        {
            EndBuffArgEvent?.Invoke(itemId);
            EndBuffNotArgEvent?.Invoke();
        }
    }

    public void StartBuff()
    {
        characterStats.AddModifier(ItemManager.Instance.itemDict[itemId].Properties);
    }
    public void EndBuff()
    {
        characterStats.RemoveModifier(ItemManager.Instance.itemDict[itemId].Properties);
    }
}
