using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Core.UI;
using DG.Tweening;

public class UIElementDailySpin : UIWindowElement
{
    private const string PopupReward = "Popup.Reward";
    private const string SlotName = "Slot.";
    private const int SlotCount = 8;

    [HideInInspector]
    public UIElementDailySpinItem WinnerItem;
    public RectTransform WinPosition;

    [SerializeField] private GameObject mParticle;
    private UIElementDailySpinItem[] mItems;
    private RectTransform Spin;

    public override void OnInitialize()
    {
        base.OnInitialize();
        InitItems();
        Spin = GetComponent<RectTransform>();
    }

    public void ResetItems()
    {

    }

    public void StartWheel()
    {
        Spin.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(1080f, 1440f)), 3.5f, RotateMode.FastBeyond360).OnComplete(FinishSpin);
        mParticle.transform.DOScale(140f, 0.7f);
        DOVirtual.DelayedCall(3.5f,()=> {
            mParticle.transform.DOScale(5f, 0.7f);
        });
    }

    private void FinishSpin()
    {
        Debug.Log("FinishSpin");
        GetWinnerElement();
        UIRoot.Instance.ShowPopup(PopupReward);
    }

    private void InitItems()
    {
        mItems = new UIElementDailySpinItem[SlotCount];
        for (int i = 0; i < SlotCount; i++)
        {
            mItems[i] = Parent.FindWindowElement(SlotName + (i + 1).ToString()) as UIElementDailySpinItem;
        }
    }

    private void GetWinnerElement()
    {
        WinnerItem = mItems[0];
        foreach (var item in mItems)
        {
            var oldDistance = Vector2.Distance(WinnerItem.ItemImage.gameObject.transform.position, WinPosition.position);
            var newDistance = Vector2.Distance(item.ItemImage.gameObject.transform.position, WinPosition.position);
            bool v = newDistance < oldDistance;
            if (v)
            {
                WinnerItem = item;
            }
        }
    }
    
}
