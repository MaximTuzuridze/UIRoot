using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.UI;

public class UIMenuPause : UIMenu
{
    private const string MenuHud = "Menu.Hud";
    private const string MenuMain = "Menu.Main";
    private const string MenuMainAnimation = "UIMainMenuShow";
    private const string PopupOptions = "Popup.Options";


    public void OnButtonHomeClick()
    {
        UIRoot.Instance.ShowMenu(MenuMain,MenuMainAnimation);
    }

    public void OnButtonOptionsClick()
    {
        UIRoot.Instance.ShowPopup(PopupOptions);
    }

    public void OnButtonContinueClick()
    {
        UIRoot.Instance.ShowMenu(MenuHud);
        UIRoot.Instance.OnGameContinue?.Invoke();
    }
}
