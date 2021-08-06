using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.UI;
using UnityEngine.UI;

public class UIWindowElementLifeBar : UIWindowElement
{
    public Image BarHpImage;

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIRoot.Instance.OnUserChangeLife += ChangeBarHp;   
    }

    public override void OnShowParentWindow()
    {
        base.OnShowParentWindow();
    }

    private void ChangeBarHp(float newVal)
    {
        BarHpImage.fillAmount = newVal;
    } 
}
