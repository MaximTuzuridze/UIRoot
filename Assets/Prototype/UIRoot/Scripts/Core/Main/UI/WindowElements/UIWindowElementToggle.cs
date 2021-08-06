using UnityEngine;
using UnityEngine.UI;
using Core.UI;
using UnityEngine.Events;
using DG.Tweening;
using Prototype;

[RequireComponent(typeof(Toggle))]
public class UIWindowElementToggle : UIWindowElement
{
    public Toggles ToggleType;
    [SerializeField] public UnityEvent OnValueChangEvent;

    public override void OnInitialize()
    {
        GetComponent<Toggle>().isOn = !IsInitiallyOn;
        GetComponent<Toggle>().onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool b)
    {
        OnValueChangEvent.Invoke();
    }

    public enum Toggles
    {
        Vibration,
        Sound,
        Music
    }


    private bool IsOn()
    {
        switch (ToggleType)
        {
            case Toggles.Sound:
                return FeedbackStorage.IsSoundsEnabled();
            case Toggles.Music:
                return FeedbackStorage.IsMusicEnabled();
            case Toggles.Vibration:
                return FeedbackStorage.IsVibroEnabled();
            default:
                return false;
        }
    }

    private bool IsInitiallyOn => IsOn();
}