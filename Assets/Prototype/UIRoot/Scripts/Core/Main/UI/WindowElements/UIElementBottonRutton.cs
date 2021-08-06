using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.UI;

public class UIElementBottonButton : UIWindowElement
{
    [SerializeField]
    private Image focus;

    [SerializeField]
    private Image unfocus;

    RectTransform rectTransform;

    private Button button;

    public override void OnInitialize()
    {
        base.OnInitialize();
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
    }

    public void Focus(bool _focus)
    {
        float kof = 2;

        if (_focus)
        {
            focus.enabled = true;

            unfocus.enabled = false;
        }
        else
        {
            focus.enabled = false;

            unfocus.enabled = true;

            kof = 0.5f;
        }
        Debug.Log("coef - " + kof);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * kof, rectTransform.sizeDelta.y);
    }
}

