using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core.UI;
using DG.Tweening;

public class UIWindowElementBar : UIWindowElement
{
    private const float _animationLength = 1f;
    
    public Image ProgressImage;

    private int currentProgress = 0;
    private float mTimer = 0;
    private bool isAnimate = false;
    private AnimationCurve progressAnimation;

    public void ShowCurrentProgress() => SetProgressLvl(GetCurrentProgress());

    public void ShowAnimatedCurrentProgress() => AnimateProgress(GetCurrentProgress());

    public void SetProgressLvl(int lvl) => ProgressImage.fillAmount = (float)(lvl - 1) / 5;

    private void AnimateProgress(int lvl)
    {
        mTimer = 0f;
        progressAnimation = new AnimationCurve();
        progressAnimation.AddKey(mTimer, (float)(lvl - 2) / 5);
        progressAnimation.AddKey(_animationLength, (float)(lvl - 2) / 5 + 0.2f);
        isAnimate = true;
    }

    private void FixedUpdate()
    {
        if (isAnimate)
            AnimateProgress();
    }

    private void AnimateProgress()
    {
        ProgressImage.fillAmount = progressAnimation.Evaluate(mTimer);
        mTimer += 0.02f;
        if (mTimer >= 1f)
            isAnimate = false;
    }

    private int GetCurrentProgress()
    {
        currentProgress = UIRoot.Instance.Storage.GetParameter<int>("CurrentLvl");
        var per = currentProgress % 5;
        if (currentProgress < 5)
            per = currentProgress;
        if (per == 0)
            per = 5;

        var currentLvl = currentProgress / 5;
        if (currentProgress % 5 == 0)
            currentLvl--;
        currentLvl++;
        UIRoot.Instance.Storage.SetParameter("UserLevel", currentLvl);
        return per;
    }
}
