using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.UI;
using TMPro;
using DG.Tweening;

public class UIWindowElementTimer : UIWindowElement
{
    public string DailyTimerId;
    public bool DailyIsAvailable;
    public TextMeshProUGUI TimerText;

    public override void OnInitialize()
    {        
        base.OnInitialize();
        DailyIsAvailable = !UIRoot.Instance.Timer.GetTimerStatus(DailyTimerId);
        CheckTimerStatus();
    }

    public void CheckTimerStatus()
    {
        DailyIsAvailable = !UIRoot.Instance.Timer.GetTimerStatus(DailyTimerId);
        TimerText.text = GetCurrentTimeLeft();
        DOVirtual.DelayedCall(1f,CheckTimerStatus);
    }

    private string GetCurrentTimeLeft()
    {
        int secondsLeft = UIRoot.Instance.Timer.GetTimerTimeLeft(DailyTimerId);
        TimeSpan ts = TimeSpan.FromSeconds(secondsLeft);
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
        ts.Hours,
        ts.Minutes,
        ts.Seconds);
    }
}
