using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Core.Utility
{
    public class UtTimer : MonoBehaviour
    {
        private const string TimersPlayerPrefKey = "TimersPlayerPrefKey";

        private readonly List<UtTime> mTimers = new List<UtTime>();


        public void CreateTimer(string id, int seconds, Action onFinish)
        {
            var time = new UtTime(id, seconds, onFinish);
            time = time.Load();
            Debug.Log("Time left: " + time.TimeInSeconds);
            mTimers.Add(time);
        }

        public void ActivateTimer(string id)
        {
            mTimers.Find(x => x.TimerId == id).IsTimerActive = true;
        }

        public bool GetTimerStatus(string timerId)
        {
            return mTimers.Find(x => x.TimerId == timerId).IsTimerActive;
        }

        public int GetTimerTimeLeft(string timerId)
        {
            return mTimers.Find(x => x.TimerId == timerId).TimeInSeconds;
        }

        public void SkipTimer(string timerId)
        {
            mTimers.Find(x => x.TimerId == timerId).TimeInSeconds = 3;
        }

        public void Initialize()
        {
            TimersUpdate();
        }

        private void OnDestroy()
        {
            foreach (var timer in mTimers)
            {
                timer.Save();
            }
        }

        private void TimersUpdate()
        {
            DOVirtual.DelayedCall(1f, TimersUpdate);
            if(mTimers.Count <= 0)
                return;

            foreach (var timer in mTimers)
            {
                timer.TimerSecondPlus();
            }
        }

        [SerializeField]
        public class UtTime
        {
            public string TimerId;
            public int TimeInSeconds;
            public Action ActionOnFinish;

            public bool IsTimerActive;

            private int TimeOnStart { get; }

            public UtTime(string name, int time, Action onFinish)
            {
                TimerId = name;
                TimeOnStart = time;
                TimeInSeconds = time;
                ActionOnFinish = onFinish;
                IsTimerActive = false;
            }

            public void TimerSecondPlus()
            {
                if (!IsTimerActive) return;
                TimeInSeconds--;
                if (TimeInSeconds > 0) return;
                ActionOnFinish.Invoke();
                IsTimerActive = false;
                TimeInSeconds = TimeOnStart;
            }

            public void Save()
            {
                this.Save(TimersPlayerPrefKey + TimerId);
            }

            public UtTime Load()
            {
                return this.Load(TimersPlayerPrefKey + TimerId);
            }
        }
    }


}