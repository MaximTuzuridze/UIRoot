using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.UI;
using TMPro;
using DG.Tweening;

public class UIElementDailyBonusItem : UIWindowElement
{
    private const string _dailyProgress = "DailyProgress";
    private const string _dailyAvailable = "DailyAvailable";

    public BonusItemState State;
    public RewardType Reward;
    public int RewardValue;

    public Image TakenImage;
    public Image TakeItGlow;
    public Image TomorrowItem;
    
    public void OnItemClick()
    {
        if (Reward == RewardType.Gem)
            UIRoot.Instance.AddGem(RewardValue);
        if (Reward == RewardType.Gold)
            UIRoot.Instance.AddGold(RewardValue);
        UIRoot.Instance.Storage.SetParameter(_dailyAvailable, false);
        var curr = UIRoot.Instance.Storage.GetParameter<int>(_dailyProgress);
        if (curr < 6)
            curr++;
        else
            curr = 0;
        UIRoot.Instance.Storage.SetParameter(_dailyProgress,  curr);
        UIRoot.Instance.Timer.ActivateTimer("ClaimDailyReward");
        AnimateClaim();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnShowParentWindow()
    {
        base.OnShowParentWindow();
        TakenImage.gameObject.SetActive(false);
        TakeItGlow.gameObject.SetActive(false);
        TomorrowItem.gameObject.SetActive(false);
        switch (State)
        {
            case BonusItemState.Available:
                TakeItGlow.gameObject.SetActive(true);
                break;
            case BonusItemState.Taken:
                TakenImage.gameObject.SetActive(true);
                break;
            case BonusItemState.Tomorrow:
                TomorrowItem.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void AnimateClaim()
    {
        TakenImage.transform.localScale = new Vector3(2, 2, 2);
        TakenImage.gameObject.SetActive(true);
        TakeItGlow.gameObject.SetActive(false);
        TakenImage.transform.DOScale(1, 1f);
        DOVirtual.DelayedCall(1.3f, () =>
        {
            UIRoot.Instance.HidePopup();
        });
    }

    public enum RewardType
    {
        Gold,
        Gem
    }
}

public enum BonusItemState
{
    Available,
    NotActive,
    Taken,
    Tomorrow
}
