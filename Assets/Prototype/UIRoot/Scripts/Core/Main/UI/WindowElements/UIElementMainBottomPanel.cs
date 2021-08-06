using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.UI;
using DanielLochner.Assets.SimpleScrollSnap;

public class UIElementMainBottomPanel : UIWindowElement
{
    private const float LeftSide = -1500;
    private const float RightSide = 1500;

    [Header("Panels configuration")]
    public MainMenuPanel[] Panels;

    [Header("Animation configuration")]
    public RectTransform MovedPanel;
    public float AnimationSpeed = 1f;

    [SerializeField]
    private ContentSizeFitter sizeFitter;
    [SerializeField] private SimpleScrollSnap scroll;

    private bool isAnimate = false;
    private float animationTime = 1f;
    private float newAnchoredPosition = 0f;
    private AnimationCurve mMenuAnimation;

    private MainMenuPanel currentPanel;

    public void OnButtonDailyClick()
    {
        scroll.GoToPanel(0);
        //ShowPanel(PanelsType.Daily);
    }

    public void OnButtonHomeClick()
    {
        scroll.GoToPanel(1);
        //ShowPanel(PanelsType.Home);
    }

    public void OnButtonShopClick()
    {
        scroll.GoToPanel(2);
       // ShowPanel(PanelsType.Shop);
    }

    public void ReInitFitter()
    {
        sizeFitter.enabled = !sizeFitter.enabled;
    }

    public void ShowPanel(PanelsType type)
    {
        var panel = GetPanel(type);
        if (currentPanel != null)
        {
            currentPanel.Button.Focus(false);
        }
        currentPanel = panel;
        currentPanel.Button.Focus(true);
        sizeFitter.enabled = !sizeFitter.enabled;
        StartAnimation(-panel.PanelPosition);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        InitializeButtons();
        currentPanel = GetPanel(PanelsType.Home);
        //OnButtonHomeClick();
    }

    private void InitializeButtons()
    {
        foreach (var panel in Panels)
        {
            panel.Init();
        }
    }
    
    private void StartAnimation(float newAnchPos)
    {
        newAnchoredPosition = newAnchPos;
        animationTime = 0f;
        mMenuAnimation = new AnimationCurve();
        mMenuAnimation.AddKey(animationTime, MovedPanel.anchoredPosition.x);
        mMenuAnimation.AddKey(AnimationSpeed,newAnchoredPosition);
        isAnimate = true;
    }

    private void AnimateMenu()
    {
        MovedPanel.anchoredPosition = new Vector2(mMenuAnimation.Evaluate(animationTime), MovedPanel.anchoredPosition.y);
        animationTime += 0.02f;
        if (animationTime >= AnimationSpeed)
        {
            isAnimate = false;
            MovedPanel.anchoredPosition = new Vector2(newAnchoredPosition, MovedPanel.anchoredPosition.y);
        }
    }

    private MainMenuPanel GetPanel(PanelsType type)
    {
        foreach (var panel in Panels)
        {
            if (panel.Type == type)
                return panel;
        }
        return null;
    }

    private void FixedUpdate()
    {
        if (isAnimate)
            AnimateMenu();
    }

    [System.Serializable]
    public class MainMenuPanel
    {
        public float AnchoredPositionX;
        public RectTransform Panel;
        public UIElementMenuButton Button;
        public PanelsType Type;
        [HideInInspector]
        public float PanelPosition => GetPosition();

        public void Init()
        {
            //Panel.anchoredPosition = new Vector2(PanelPosition, Panel.anchoredPosition.y);
        }

        private float GetPosition()
        {
            float minVal = Screen.width > 1500f ? Screen.width : 1500f;
            return AnchoredPositionX * minVal;
        }
    }

    public enum PanelsType {
        Home,
        Daily,
        Shop
    }

}
