using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using Core.Utility;
using Prototype.AudioCore;

namespace Core.UI
{
    public abstract class UIBase<Instance> : MonoBehaviourSingleton<Instance> where Instance : MonoBehaviour
    {
        private const string _uiMenuDefaultAnimationShow = "UIMenuShow";
        private const string _uiMenuDefaultAnimationHide = "UIMenuHide";
        private const string _uiPopupDefaultAnimationShow = "UIPopupShow";
        private const string _uiPopupDefaultAnimationHide = "UIPopupHide";

        [Space(5)] [Header("UIRoot Debug mode status")]
        public bool IsDebugModeActive = false;

        [Space(5)] public List<UIMenu> UiWindowsMenu;

        public List<UIPopup> UiWindowsPopups;

        [HideInInspector]
        public UtStorage Storage;
        [HideInInspector]
        public UtTimer Timer;

        public Action OnGameStart;
        public Action OnGamePause;
        public Action OnGameContinue;

        private UIMenu currentUiMenu;
        private UIPopup currentUiPopup;
        private List<UIPopup> activePopups;

        private UtHolder utHolder;

        protected virtual void OnInitialize() { }

        protected virtual void ParameterInitialization() { }

        protected virtual void TimerInitialization() { }

        public void Init()
        {
            AudioController.InitStreams(gameObject);
            LanguageAudio.Init();
            InitializeUtility();
            InitializeMenus();
            InitializePopups();
            OnInitialize();
        }

        public void ShowMenu(string menuName, string animationName = _uiMenuDefaultAnimationShow)
        {
            if (currentUiMenu == null)
            {
                currentUiMenu = GetUiMenu(menuName);
                ShowCurrentWindowMenu(animationName);
            }
            else
            {
                HideCurrentWindowMenu();
                currentUiMenu = GetUiMenu(menuName);
                ShowCurrentWindowMenu(animationName);
            }
        }

        public void ShowPopup(string menuName, string animationName = _uiPopupDefaultAnimationShow)
        {
            currentUiPopup = GetUiPopup(menuName);
            currentUiPopup.ShowWindow(animationName);
            activePopups.Add(currentUiPopup);
        }

        public UIMenu FindMenu(string menu)
        {
            return UiWindowsMenu.FirstOrDefault(m => m.WindowId == menu);
        }

        public UIPopup FindPopup(string popup)
        {
            return UiWindowsPopups.FirstOrDefault(p => p.WindowId == popup);
        }

        public void HidePopup(string animationName = _uiPopupDefaultAnimationHide)
        {
            activePopups.LastOrDefault()?.HideWindow(animationName);
        }

        private void ShowCurrentWindowMenu(string animName)
        {
            currentUiMenu.ShowWindow(animName);
        }

        private void HideCurrentWindowMenu(string animName = _uiMenuDefaultAnimationHide)
        {
            currentUiMenu.HideWindow(animName);
        }

        private UIMenu GetUiMenu(string id)
        {
            return UiWindowsMenu.FirstOrDefault(window => window.WindowId == id);
        }

        private UIPopup GetUiPopup(string id)
        {
            return UiWindowsPopups.FirstOrDefault(window => window.WindowId == id);
        }

        private void InitializeMenus()
        {
            foreach (var wind
                in GetComponentsInChildren<UIMenu>(true))
            {
                if (wind.IsWindowActive)
                    UiWindowsMenu.Add(wind);
                wind.gameObject.SetActive(false);
            }

            foreach (var menu in UiWindowsMenu)
            {
                menu.Init();
            }
        }

        private void InitializePopups()
        {
            foreach (var wind
                in GetComponentsInChildren<UIPopup>(true))
            {
                if (wind.IsWindowActive)
                    UiWindowsPopups.Add(wind);
                wind.gameObject.SetActive(false);
            }

            activePopups = new List<UIPopup>();
            
            foreach (var popup in UiWindowsPopups)
            {
                popup.Init();
            }
        }

        private void InitializeUtility()
        {
            //add holder for Utility
            utHolder = Instantiate(new GameObject("UtilityHolder"), gameObject.transform).AddComponent<UtHolder>();

            //add and initialize storage utility
            Storage = utHolder.AddUtility("Storage").AddComponent<UtStorage>();
            Storage.Initialize();
            ParameterInitialization();
            //add and initialize timer utility
            Timer = utHolder.AddUtility("Timer").AddComponent<UtTimer>();
            Timer.Initialize();
            TimerInitialization();
        }
    }
}