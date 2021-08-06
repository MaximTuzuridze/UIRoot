using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.UI;
using Prototype;

public class UIPopupOptions : UIPopup
{
    public void OnButtonCloseClick()
    {
        UIRoot.Instance.HidePopup();
    }

    public void OnToggleSound()
    {
        FeedbackStorage.EnableSounds(!FeedbackStorage.IsSoundsEnabled());
    }

    public void OnToggleMusic()
    {
        FeedbackStorage.EnableMusic(!FeedbackStorage.IsMusicEnabled());
    }

    public void OnToggleVibration()
    {
        FeedbackStorage.EnableVibro(!FeedbackStorage.IsVibroEnabled());
    }
}
