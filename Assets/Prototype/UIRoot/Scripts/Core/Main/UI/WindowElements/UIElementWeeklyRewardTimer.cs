using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.UI;

public class UIElementWeeklyRewardTimer : UIWindowElement
{
    private const string weeklyRewardId = "WeeklyRewardProgress";
    private const float AnimationProgressLength = 0.5f;

    public WeeklyProgressItem[] Progress;

    private int progress;
    private float animationTimer;
    private bool isAnimate;
    private WeeklyProgressItem currentProgressItem;
    private AnimationCurve progressAnimation;

    public override void OnInitialize()
    {
        base.OnInitialize();
        progress = UIRoot.Instance.Storage.GetParameter<int>(weeklyRewardId);
        SetProgress(progress);
        UIRoot.Instance.Storage.OnParameterChange<int>(weeklyRewardId, OnWeeklyGet);
    }

    private void OnWeeklyGet(int progress)
    {
        animationTimer = 0;
        currentProgressItem = getItem(progress);
        progressAnimation = new AnimationCurve();
        progressAnimation.AddKey(0f, 0f);
        progressAnimation.AddKey(AnimationProgressLength, 1f);
        isAnimate = true;
    }

    private WeeklyProgressItem getItem(int progress)
    {
        foreach (var item in Progress)
        {
            if (item.Id == progress)
                return item;
        }
        return null;
    }

    private void SetProgress(int progress)
    {
        foreach (var item in Progress)
        {
            if (item.Id <= progress)
                item.Image.fillAmount = 1;
            else
                item.Image.fillAmount = 0;
        }
    }

    private void AnimateProgress()
    {
        currentProgressItem.Image.fillAmount = progressAnimation.Evaluate(animationTimer);
        animationTimer += 0.02f;
        if (animationTimer >= AnimationProgressLength)
        {
            isAnimate = false;
            currentProgressItem.Image.fillAmount = 1f;
            currentProgressItem.IsFinished = true;
        }
    }

    private void FixedUpdate()
    {
        if (isAnimate)
            AnimateProgress();
    }

    [System.Serializable]
    public class WeeklyProgressItem
    {
        public int Id;
        public Image Image;
        [HideInInspector]
        public bool IsFinished;

    }
}
