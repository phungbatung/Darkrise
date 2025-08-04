using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPresenter : MonoBehaviour
{
    public BuffModel buffModel { get; private set; }
    private BuffView buffView { get; set; }

    public void ExecuteCountDown(float _deltaTime)
    {
        buffModel.Countdown(_deltaTime);
    }
    public void UpdateView(float _timeLeft)
    {
        buffView.SetTimeLeft(_timeLeft);
    }

    public void EndBuff()
    {
        buffView?.EndBuff();
        Destroy(gameObject);
    }

    public void Setup(BuffModel _buffModel, BuffView _buffView)
    {
        buffModel = _buffModel;
        buffView = _buffView;
        buffView.SetIcon(ItemManager.Instance.itemDict[buffModel.itemId].Icon);
        buffModel.EndBuffNotArgEvent += EndBuff;
        buffModel.CountdownEvent += UpdateView;
    }
}
