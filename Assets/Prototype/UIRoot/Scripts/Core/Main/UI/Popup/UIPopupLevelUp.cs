using System.Collections;
using System.Collections.Generic;
using Core.UI;
using UnityEngine.UI;
using TMPro;

public class UIPopupLevelUp : UIPopup
{
    private const string _rewardGold = "LevelUpRewardGold";
    private const string _rewardGem = "LevelUpRewardGem";
    private const string _currentGold = "CurrentGold";
    private const string _currentGem = "CurrentGem";

    public void OnButtonClaimClick()
    {
        ClaimReward();
        UIRoot.Instance.HidePopup();
    }

    private void ClaimReward()
    {
        UIRoot.Instance.Storage.SetParameter(_currentGold, CurrentGold + RewardGold);
        UIRoot.Instance.Storage.SetParameter(_currentGem, CurrentGem + RewardGem);
        UIRoot.Instance.Storage.SetParameter(_rewardGold, RewardGold + RewardGold);
        UIRoot.Instance.Storage.SetParameter(_rewardGem, RewardGem + RewardGem);
    }

    private int CurrentGold => UIRoot.Instance.Storage.GetParameter<int>(_currentGold);
    private int CurrentGem => UIRoot.Instance.Storage.GetParameter<int>(_currentGem);
    private int RewardGold => UIRoot.Instance.Storage.GetParameter<int>(_rewardGold);
    private int RewardGem => UIRoot.Instance.Storage.GetParameter<int>(_rewardGem);
}
