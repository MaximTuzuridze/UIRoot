using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Core.UI
{
    [RequireComponent(typeof(Animation))]
    public abstract class UIWindow : MonoBehaviour
    {
        public bool IsWindowActive = true;
        public string WindowId => Id();
        public IEnumerable<UIWindowElement> WindowElements => FindElements();
        public Action OnFinishHideWindow;

        private const float _eventDelay = 0.3f;

        private Animation animator;
        private string windowName;
        private IEnumerable<UIWindowElement> mWindowElements;
        private Tween tweenDelayOnHide;


        public virtual void OnInitialize() { }

        public virtual void OnShow() { }

        public virtual void OnHide() { }

        public void ShowWindow(string animationName)
        {
            gameObject.SetActive(true);
            tweenDelayOnHide?.Kill();
            animator.Play(animationName);

            OnShow();
            foreach (var element in WindowElements)
            {
                element.OnShowParentWindow();
            }
        }

        public void HideWindow(string animationName)
        {
            var clip = animator.GetClip(animationName);
            var time = clip.length + _eventDelay;
            tweenDelayOnHide = DOVirtual.DelayedCall(time, FinishHideWindow);
            animator.Play(animationName);

            OnHide();
        }

        public void Init()
        {
            animator = GetComponent<Animation>();
            foreach (var windowElement in WindowElements)
            {
                windowElement.Initialize(this);
            }
            OnInitialize();
        }

        private string Id()
        {
            if(!string.IsNullOrEmpty(windowName))
                return windowName;
            windowName = this.name;

            return windowName;
        }

        public UIWindowElement FindWindowElement(string elementId)
        {
            return WindowElements.FirstOrDefault(windowElement => windowElement.ElementId == elementId);
        }

        private IEnumerable<UIWindowElement> FindElements()
        {
            return mWindowElements ?? (mWindowElements = GetComponentsInChildren<UIWindowElement>());
        }

        private void FinishHideWindow()
        {
            OnFinishHideWindow?.Invoke();
            gameObject.SetActive(false);
        }
    }
}