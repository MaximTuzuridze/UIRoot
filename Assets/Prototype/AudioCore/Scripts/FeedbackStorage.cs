using UnityEngine;
using Prototype.AudioCore;

namespace Prototype
{
    public static class FeedbackStorage
    {
        private static bool _actual;

        private static bool _vibroON = true;
        private static bool _musicON = true;
        private static bool _soundsON = true;
        private static float _musicVol = 1;
        private static float _soundsVol = 1;
        private static int _currentLang = -1;
        
        public static void UpdateSettings()
        {
            if (!_actual)
            {
                _vibroON = PlayerPrefs.GetInt("vibro_on", 1) != 0;
                _musicON = PlayerPrefs.GetInt("music_on", 1) != 0;
                _soundsON = PlayerPrefs.GetInt("sounds_on", 1) != 0;
                _musicVol = PlayerPrefs.GetFloat("music_vol", 1);
                _soundsVol = PlayerPrefs.GetFloat("sounds_vol", 1);
                _currentLang = PlayerPrefs.GetInt("CurrentLanguage", -1);
                _actual = true;
            }
        }
        
        public static bool IsVibroEnabled()
        {
            return _vibroON;
        }
        
        public static void EnableVibro(bool param)
        {
            _vibroON = param;
            PlayerPrefs.SetInt("vibro_on", _vibroON == false ? 0 : 1);
            SaveSettings();
        }
        
        public static bool IsMusicEnabled()
        {
            return _musicON;
        }
        
        public static void EnableMusic(bool param)
        {
            _musicON = param;
            AudioController.EnableMusic(_musicON);
            PlayerPrefs.SetInt("music_on", (_musicON == false ? 0 : 1));
            SaveSettings();
        }
        
        public static bool IsSoundsEnabled()
        {
            return _soundsON;
        }
        
        public static void EnableSounds(bool param)
        {
            _soundsON = param;
            AudioController.EnableSounds(_soundsON);
            PlayerPrefs.SetInt("sounds_on", (_soundsON == false ? 0 : 1));
            SaveSettings();
        }
        
        public static float GetMusicVol()
        {
            return _musicVol;
        }
        
        public static void SetMusicVol(float param)
        {
            _musicVol = param;
            AudioController.SetMusicVolume(_musicVol);
            PlayerPrefs.SetFloat("music_vol", _musicVol);
            SaveSettings();
        }
        
        public static float GetSoundsVol()
        {
            return _soundsVol;
        }
        
        public static void SetSoundsVol(float param)
        {
            _soundsVol = param;
            AudioController.SetSoundsVolume(_soundsVol);
            PlayerPrefs.SetFloat("sounds_vol", _soundsVol);
            SaveSettings();
        }
        
        public static int GetCurrentLang()
        {
            return _currentLang;
        }
        
        public static void SetCurrentLang(int lang)
        {
            _currentLang = lang;
            PlayerPrefs.SetInt("CurrentLanguage", _currentLang);
            SaveSettings();
        }

        private static void SaveSettings()
        {
            PlayerPrefs.Save();
        }
    }
}
