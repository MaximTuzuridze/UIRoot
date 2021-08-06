using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.UI;

public class UIElementMenuButton : UIWindowElement
{
    private const float disabledX = 375;
    private const float enabledX = 750;

    [SerializeField]
    private Image focus;

    [SerializeField]
    private Image unfocus;

    [SerializeField]
    private Image selected;

    RectTransform rectTransform;
    
    public override void OnInitialize()
    {
        base.OnInitialize();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Focus(bool _focus)
    {
        float sizeX = _focus ? enabledX : disabledX;
        if (_focus)
        {
            focus.enabled = true;
            unfocus.enabled = false;
            selected.enabled = true;
        }
        else
        {
            focus.enabled = false;
            unfocus.enabled = true;
            selected.enabled = false;
        }
        if (rectTransform)
        rectTransform.sizeDelta = new Vector2(sizeX, rectTransform.sizeDelta.y);
    }
}
