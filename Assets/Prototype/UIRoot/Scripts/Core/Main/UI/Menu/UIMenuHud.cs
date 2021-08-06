using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.UI;

public class UIMenuHud : UIMenu
{
    private const string MenuPause = "Menu.Pause";

    public void OnButtonPauseClick()
    {
        UIRoot.Instance.ShowMenu(MenuPause);
        UIRoot.Instance.OnGamePause?.Invoke();
    }

    public void DebugWin()
    {
        UIRoot.Instance.FinishGame(true);
    }

    public void DebugLose()
    {
        UIRoot.Instance.FinishGame(false);
    }

    public void DebugAddGold()
    {
        UIRoot.Instance.AddGold(5);
    }

    public void DebugAddGem()
    {
        UIRoot.Instance.AddGem(5);
    }

    public void DebugResetSpin()
    {
        UIRoot.Instance.Timer.SkipTimer("DailyBonusWheel");
    }

    public void DebugResetDaily()
    {
        UIRoot.Instance.Timer.SkipTimer("ClaimDailyReward");
    }

    public void DebugGetDamage()
    {
        UIRoot.Instance.UserGetDamage();
    }
}
