using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using Core.Utility;
using Core.UI;

public class UIRoot : UIBase<UIRoot>
{
    private const string _mainMenuName = "Menu.Main";
    private const string _winGameMenuName = "Menu.WinGame";
    private const string _loseGameMenuName = "Menu.LoseGame";
    private const string _mainMenuShowAnimation = "UIMainMenuFirstShow";
    private const string _popupLevelUp = "Popup.LevelUp";

    [Header("User Maximal HP")] public int UserMaxLifeCount = 3;

    public Action<float> OnUserChangeLife;
    public Action OnGaveRareReward;

    private int currentUserLife;

    #region UI_Root_Interaction

    protected override void OnInitialize()
    {
        OnGameStart += ResetUserLife;
        ShowMenu(_mainMenuName, _mainMenuShowAnimation);
    }

    public void FinishGame(bool userWin)
    {
        if (userWin)
            Storage.SetParameter<int>("CurrentLvl", Storage.GetParameter<int>("CurrentLvl") + 1);
        ShowMenu(userWin ? _winGameMenuName : _loseGameMenuName);
        if (userWin && Storage.GetParameter<int>("CurrentLvl") % 5 == 1)
            ShowPopup(_popupLevelUp);
    }

    public void AddGold(int gold)
    {
        var newVal = Storage.GetParameter<int>("CurrentGold") + gold;
        Storage.SetParameter<int>("CurrentGold", newVal);
    }

    public void AddGem(int gem)
    {
        var newVal = Storage.GetParameter<int>("CurrentGem") + gem;
        Storage.SetParameter<int>("CurrentGem", newVal);
    }

    public void UserGetDamage(int damage = 1)
    {
        currentUserLife -= damage;
        var val = ((float) currentUserLife / (float) UserMaxLifeCount);
        OnUserChangeLife?.Invoke((currentUserLife > 0) ? val : 0);
    }

    public void GiveRareReward()
    {
        OnGaveRareReward?.Invoke();
    }

    #endregion

    protected override void ParameterInitialization()
    {
        Storage.AddParameter("CurrentLvl", 1);
        Storage.AddParameter("CurrentGold", 0);
        Storage.AddParameter("CurrentGem", 0);
        Storage.AddParameter("CurrentDailyReward", 0);
        Storage.AddParameter("DailyWheelAvailable", true);
        Storage.AddParameter("DailyProgress", 0);
        Storage.AddParameter("DailyAvailable", true);
        Storage.AddParameter("WeeklyRewardProgress", 0);
        Storage.AddParameter("MainMenuBGActive", true);
        Storage.AddParameter("UserLevel", 1);
        Storage.AddParameter("LevelUpRewardGold", 500);
        Storage.AddParameter("LevelUpRewardGem", 5);
    }

    protected override void TimerInitialization()
    {
        Timer.CreateTimer("DailyBonusWheel", 3600, () => { Instance.Storage.SetParameter("DailyWheelAvailable", true); });
        Timer.CreateTimer("ClaimDailyReward", 86400, () => { Instance.Storage.SetParameter("DailyAvailable", true); });

    }

    private void ResetUserLife()
    {
        currentUserLife = UserMaxLifeCount;
        OnUserChangeLife?.Invoke(currentUserLife);
    }
}

