using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Core.UI;
using DG.Tweening;

public class UIElementDailyBonus : UIWindowElement
{
    private const string _rewardName = "RewardDay";
    private const string _dailyProgress = "DailyProgress";
    private const string _dailyAvailable = "DailyAvailable";
    private const string _claimButton = "Button_Claim";
    private const string _timerDaily = "Timer";

    public UIElementDailyBonusItem[] Items;

    private Action onClaimReward;

    public override void OnInitialize()
    {
        base.OnInitialize();
        InitItems();
    }

    public override void OnShowParentWindow()
    {
        base.OnShowParentWindow();
        onClaimReward = null;
        SetAvailable();
    }

    public void ClaimReward()
    {
        onClaimReward?.Invoke();
    }

    private void InitItems()
    {
        Items = new UIElementDailyBonusItem[7];
        for (int i = 0; i < 7; i++)
            Items[i] = Parent.FindWindowElement(_rewardName + (i + 1).ToString()) as UIElementDailyBonusItem;
        SetAvailable();
    }

    private void SetAvailable()
    {
        var button = Parent.FindWindowElement(_claimButton) as UIWindowElementButton;
        button.Disable();
        var timer = Parent.FindWindowElement(_timerDaily) as UIWindowElementTimer;
        timer.Disable();
        foreach (var item in Items)
            item.State = BonusItemState.NotActive;

        for (int i = 0; i < 7; i++)
            if (DailyProgress > i)
                Items[i].State = BonusItemState.Taken;

        if (DailyAvailable)
        {
            Items[DailyProgress].State = BonusItemState.Available;
            onClaimReward = Items[DailyProgress].OnItemClick;
            button.Enable();
        }
        else
        {
            Items[DailyProgress].State = BonusItemState.Tomorrow;
            timer.Enable();
        }
    }

    private bool DailyAvailable => UIRoot.Instance.Storage.GetParameter<bool>(_dailyAvailable);
    private int DailyProgress => UIRoot.Instance.Storage.GetParameter<int>(_dailyProgress);
}
