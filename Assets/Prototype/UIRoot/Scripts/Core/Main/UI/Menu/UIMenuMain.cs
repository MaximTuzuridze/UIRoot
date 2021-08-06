using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.UI;
using Core.Utility;

public class UIMenuMain : UIMenu
{
    private const string _menuHud = "Menu.Hud";
    private const string _menuDaily = "Menu.Daily";
    private const string _popupOptions = "Popup.Options";
    private const string _popupDailyReward = "Popup.DailyReward";
    private const string _progressBar = "Level";
    private const string _spin = "Spin";
    private const string _buttonSpin = "Button_Spin";
    private const string _dailyTimerElement = "Timer";
    private const string _dailySpinAvailable = "DailyWheelAvailable";
    private const string _dailyAvailable = "DailyAvailable";

    public Image DailyAvailableImage;

    private UIElementDailySpin DailySpin;    
    private UIWindowElementBar bar;
    private UIWindowElementButton dailyButton;
    private UIWindowElementTimer dailyTimer;

    public override void OnInitialize()
    {
        base.OnInitialize();
        DailySpin = FindWindowElement(_spin) as UIElementDailySpin;
        dailyButton = FindWindowElement(_buttonSpin) as UIWindowElementButton;
        dailyTimer = FindWindowElement(_dailyTimerElement) as UIWindowElementTimer;
        UIRoot.Instance.Storage.OnParameterChange<bool>(_dailySpinAvailable, SetDailyWheel);
        UIRoot.Instance.Storage.OnParameterChange<bool>(_dailyAvailable, SetDaily);
    }

    public override void OnShow()
    {
        base.OnShow();
        bar = FindWindowElement(_progressBar) as UIWindowElementBar;
        bar?.ShowCurrentProgress();
        SetDailyWheel(UIRoot.Instance.Storage.GetParameter<bool>(_dailySpinAvailable));
        SetDaily(UIRoot.Instance.Storage.GetParameter<bool>(_dailyAvailable));
        DailySpin.ResetItems();
    }

    public void OnButtonStartGameClick()
    {
        UIRoot.Instance.ShowMenu(_menuHud);
        UIRoot.Instance.OnGameStart?.Invoke();
    }

    public void OnButtonOptionsClick() => UIRoot.Instance.ShowPopup(_popupOptions);

    public void OnButtonDailyRewardClick() => UIRoot.Instance.ShowPopup(_popupDailyReward);

    public void OnButtonDailyRollClick() => DailySpin.StartWheel();

    private void SetDailyWheel(bool dailyAtive)
    {
        if (dailyAtive)
        {
            dailyTimer.Disable();
            dailyButton.Enable();
        }
        else
        {
            dailyTimer.Enable();
            dailyButton.Disable();
        }
    }

    private void SetDaily(bool dailyAtive) => DailyAvailableImage.gameObject.SetActive(dailyAtive);
}