using System.Collections;
using System.Collections.Generic;
using Core.UI;
using UnityEngine.UI;
using TMPro;

public class UIPopupDailyReward : UIPopup
{
    public void OnButtonCloseClick()
    {
        UIRoot.Instance.HidePopup();
    }

    public void OnButtonClaimClick()
    {
        UIRoot.Instance.HidePopup();
    }

    private int GetcurrentGold => UIRoot.Instance.Storage.GetParameter<int>("CurrentGold");
}
