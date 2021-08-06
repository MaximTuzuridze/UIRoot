using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.UI;

public class UIMenuLoseGame : UIMenu
{
    private const string MenuHud = "Menu.Hud";
    private const string MenuMain = "Menu.Main";
    private const string MenuMainAnimation = "UIMainMenuShow";
    private const string ProgressBar = "Level";

    private UIWindowElementBar bar;
    
    public override void OnShow()
    {
        base.OnShow();
        bar = FindWindowElement(ProgressBar) as UIWindowElementBar;
        bar?.ShowCurrentProgress();
    }

    public void OnButtonHomeClick()
    {
        UIRoot.Instance.ShowMenu(MenuMain, MenuMainAnimation);
    }

    public void OnButtonRetryClick()
    {
        UIRoot.Instance.ShowMenu(MenuHud);
        UIRoot.Instance.OnGameStart?.Invoke();
    }
}
