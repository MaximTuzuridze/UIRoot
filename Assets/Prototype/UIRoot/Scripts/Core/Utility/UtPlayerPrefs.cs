using UnityEngine;
using System;

namespace Core.Utility
{
    public static class UtPlayerPrefs
    {
        private static int MaximalDateDelay { get; } = 604800;

        #region Saving Methods

        public static void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public static string Load(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public static void Save(this object obj, string key)
        {
            if (obj is string)
            {
                Save(obj.ToString(), key);
                return;
            }
            var value = obj.SerializeToString();
            PlayerPrefs.SetString(key, value);
        }

        public static void Save(this UtTimer.UtTime time, string key)
        {
            var value = time.IsTimerActive.SerializeToString();
            PlayerPrefs.SetString(key + "_isActive", value);
            var value2 = time.TimeInSeconds.SerializeToString();
            PlayerPrefs.SetString(key + "_timeLeft", value2);
            var value3 = DateTime.Now.ToBinary().ToString();
            PlayerPrefs.SetString(key + "_currentTime", value3);
        }

        public static T Load<T>(this MonoBehaviour Mono, string key)
        {
            var str = PlayerPrefs.GetString(key);
            return !string.IsNullOrEmpty(str) ? str.DeserializeString<T>() : default;
        }

        public static UtTimer.UtTime Load(this UtTimer.UtTime time, string key)
        {
            var newTime = time;
            var str = PlayerPrefs.GetString(key + "_isActive");
            newTime.IsTimerActive = !string.IsNullOrEmpty(str) && str.DeserializeString<bool>();
            if (!newTime.IsTimerActive)
                return newTime;

            str = PlayerPrefs.GetString(key + "_timeLeft");
            newTime.TimeInSeconds = str.DeserializeString<int>();
            str = PlayerPrefs.GetString(key + "_currentTime");
            var temp = Convert.ToInt64(str);
            var oldDate = DateTime.FromBinary(temp);
            var currentTime = DateTime.Now.Subtract(oldDate).TotalSeconds;
            if (currentTime > MaximalDateDelay)
                currentTime = MaximalDateDelay;
            newTime.TimeInSeconds -= (int)currentTime;
            return newTime;
        }

        public static T Load<T>(string key)
        {
            var str = PlayerPrefs.GetString(key);
            return !string.IsNullOrEmpty(str) ? str.DeserializeString<T>() : default(T);
        }

        public static bool Load<T>(string key, out T result)
        {
            var str = PlayerPrefs.GetString(key);
            if (!string.IsNullOrEmpty(str))
            {
                result = str.DeserializeString<T>();

                Debug.Log("load:" + result);
                return true;
            }

            result = default;
            return false;
        }

        #endregion

        private static void Save(this string value, string key)
        {
            PlayerPrefs.SetString(key, value);
        }
    }
}