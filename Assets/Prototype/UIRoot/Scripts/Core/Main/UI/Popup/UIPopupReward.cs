using System.Collections;
using System.Collections.Generic;
using Core.UI;
using UnityEngine.UI;
using TMPro;

public class UIPopupReward : UIPopup
{
    private const string _mainMenuName = "Menu.Main";
    private const string _dailyWheelAvailable = "DailyWheelAvailable";
    private const string _dailyBonusWheelTimer = "DailyBonusWheel";
    private const string _currentGoldParameter = "CurrentGold";
    private const string _currentGemParameter = "CurrentGem";
    private const string _spin = "Spin";

    public TextMeshProUGUI RewardText;
    public Image Image;

    private int reward;
    private UIMenuMain main;
    private UIElementDailySpin spin;

    public override void OnShow()
    {
        main = UIRoot.Instance.FindMenu(_mainMenuName) as UIMenuMain;
        spin = main.FindWindowElement(_spin) as UIElementDailySpin;
        base.OnShow();
        reward = spin.WinnerItem.Reward;
        RewardText.text = spin.WinnerItem.Item != SpinItemType.Rare ? "x" + reward.ToString() : "RARE";
        Image.sprite = spin.WinnerItem.ItemImage.sprite;
    }

    public void OnButtonClaimClick()
    {
        AddReward();
        UIRoot.Instance.Storage.SetParameter(_dailyWheelAvailable, false);
        UIRoot.Instance.Timer.ActivateTimer(_dailyBonusWheelTimer);
        UIRoot.Instance.HidePopup();
    }

    private void AddReward()
    {
        switch (spin.WinnerItem.Item)
        {
            case SpinItemType.Gold:
                UIRoot.Instance.Storage.SetParameter(_currentGoldParameter, GetcurrentGold + reward);
                break;
            case SpinItemType.Gem:
                UIRoot.Instance.Storage.SetParameter(_currentGemParameter, GetcurrentGems + reward);
                break;
            case SpinItemType.Rare:
                UIRoot.Instance.GiveRareReward();
                break;
            default:
                break;
        }
    }

    private int GetcurrentGold => UIRoot.Instance.Storage.GetParameter<int>(_currentGoldParameter);

    private int GetcurrentGems => UIRoot.Instance.Storage.GetParameter<int>(_currentGemParameter);
}
