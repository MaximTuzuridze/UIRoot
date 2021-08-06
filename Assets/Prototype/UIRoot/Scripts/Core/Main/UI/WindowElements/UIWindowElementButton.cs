using UnityEngine;
using UnityEngine.UI;
using Core.UI;
using UnityEngine.Events;
using Prototype.AudioCore;

[RequireComponent(typeof(Button))]
public class UIWindowElementButton : UIWindowElement
{
    [SerializeField] public UnityEvent OnInitializeEvent;
    [SerializeField] public UnityEvent OnClickEvent;

    public override void OnInitialize()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        OnInitializeEvent?.Invoke();
    }

    private void OnClick()
    {
        OnClickEvent?.Invoke();
        AudioController.PlaySound("ButtonClick");
    }
}