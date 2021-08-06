#define BRAND_NEW_PROTOTYPE

using UnityEngine;
using System.Collections.Generic;

namespace Prototype.AudioCore
{
    public class Languages : MonoBehaviour
    {
        private static int _localeIndex;
        private static string _localeName;
        private static Language _localeLang;

        public delegate void Callback(Language lang);
        public static event Callback OnLangChange;

        public enum Language
        {
            Arabic = SystemLanguage.Arabic,
            Chinese = SystemLanguage.Chinese,
            Czech = SystemLanguage.Czech,
            Danish = SystemLanguage.Danish,
            Dutch = SystemLanguage.Dutch,
            English = SystemLanguage.English,
            Finnish = SystemLanguage.Finnish,
            French = SystemLanguage.French,
            German = SystemLanguage.German,
            Greek = SystemLanguage.Greek,
            Italian = SystemLanguage.Italian,
            Japanese = SystemLanguage.Japanese,
            Norwegian = SystemLanguage.Norwegian,
            Polish = SystemLanguage.Polish,
            Portuguese = SystemLanguage.Portuguese,
            Romanian = SystemLanguage.Romanian,
            Russian = SystemLanguage.Russian,
            Spanish = SystemLanguage.Spanish,
            Swedish = SystemLanguage.Swedish,
            Turkish = SystemLanguage.Turkish,
            Hindi = 50,
            Hebrew = SystemLanguage.Hebrew,
            Indonesian = SystemLanguage.Indonesian,
            Korean = SystemLanguage.Korean,
            Thai = SystemLanguage.Thai,
            Ukrainian = SystemLanguage.Ukrainian,
            Catalan = SystemLanguage.Catalan,
            Belarusian = SystemLanguage.Belarusian,
        };

        public static readonly Dictionary<Language, Language[]> LanguagesOverride = new Dictionary<Language, Language[]>()
        {
            { Language.Russian, new Language[] {Language.Ukrainian, Language.Belarusian } }, //"key" will replace any item from "value"
            { Language.Spanish , new Language[] {Language.Catalan } }, //"key" will replace any item from "value"
        };

        public static readonly Dictionary<Language, string> languages = new Dictionary<Language, string>()
        {
		    //{ Language.Arabic, "Arabic" },
		    //{ Language.Chinese, "Chinese" },
		    //{ Language.Czech, "Czech" },
		    //{ Language.Danish, "Danish" },
		    //{ Language.Dutch, "Dutch" },
		    { Language.English, "English" },
		    //{ Language.Finnish, "Finnish" },
            //{ Language.French, "French" },
            //{ Language.German, "German" },
		    //{ Language.Greek, "Greek" },
		    //{ Language.Hebrew, "Hebrew" },
		    //{ Language.Hindi, "Hindi" },
		    //{ Language.Indonesian, "Indonesian" },
		    //{ Language.Italian, "Italian" },
		    //{ Language.Japanese, "Japanese" },
		    //{ Language.Korean, "Korean" },
		    //{ Language.Norwegian, "Norwegian" },
		    //{ Language.Polish, "Polish" },
            //{ Language.Portuguese, "Portuguese" },
		    //{ Language.Romanian, "Romanian" },
		    { Language.Russian, "Russian" },
            //{ Language.Spanish, "Spanish" },
		    //{ Language.Swedish, "Swedish" },
		    //{ Language.Thai, "Thai" },
		    //{ Language.Turkish, "Turkish" },
		    //{ Language.Ukrainian, "Ukrainian" },
        };

        public static readonly string[] Localization = 
        {
            "Arabic",
            "Chinese",
            "Czech",
            "Danish",
            "Dutch",
            "English",
            "Finnish",
            "French",
            "German",
            "Greek",
            "Hebrew",
            "Hindi",
            "Indonesian",
            "Italian",
            "Japanese",
            "Korean",
            "Norwegian",
            "Polish",
            "Portuguese",
            "Romanian",
            "Russian",
            "Spanish",
            "Swedish",
            "Thai",
            "Turkish",
            "Ukrainian"
        };

        private static void CheckOverrides(ref Language param)
        {
            foreach (var rule in LanguagesOverride)
            {
                for (var i = 0; i < rule.Value.Length; i++)
                {
                    if (rule.Value[i].Equals(param))
                    {
                        param = rule.Key;
                        break;
                    }
                }
            }
        }

        public static void Init()
        {
            var lang = Language.English;
#if BRAND_NEW_PROTOTYPE
            LanguageAudio.Init();
            if (FeedbackStorage.GetCurrentLang() >= 0)
            {
                lang = (Language)FeedbackStorage.GetCurrentLang();
            }
#else
        if (PlayerPrefs.HasKey ("CurrentLanguage")) {
			lang = (Language)PlayerPrefs.GetInt ("CurrentLanguage");
		} 
#endif
            else
            {
                lang = DetectLanguage();
            }
            SetLanguage(lang);
        }
        
        private static Language DetectLanguage()
        {
            var lang = Utils.GetSystemLanguage();
            CheckOverrides(ref lang);
            if (languages.ContainsKey(lang))
            {
                return lang;
            }
            else
            {
                return Language.English;
            }
        }
        
        public static void SetLanguageByName(string lname)
        {
            foreach (var pair in languages)
            {
                if (pair.Value == lname)
                {
                    SetLanguage(pair.Key);
                    break;
                }
            }
        }
        
        public static void SetLanguage(Language lang)
        {
            if (languages.ContainsKey(lang))
            {
                var name = languages[lang];
                for (var i = 0; i < Localization.Length; i++)
                {
                    if (Localization[i] == name)
                    {
                        _localeIndex = i;
                        _localeName = Localization[i];
                        _localeLang = lang;
                        break;
                    }
                }
#if BRAND_NEW_PROTOTYPE
                FeedbackStorage.SetCurrentLang((int)_localeLang);
                LanguageAudio.LoadSounds(true, false);
#else
            PlayerPrefs.SetInt ( "CurrentLanguage", (int) locale_lang );
#endif
                if (OnLangChange != null)
                {
                    OnLangChange(_localeLang);
                }
            }
            else
            {
                Debug.LogError("Couldn't set langueage " + lang.ToString());
            }
        }

        public static void NextLanguage()
        {
            _localeIndex++;
            if (_localeIndex > Localization.Length - 1)
            {
                _localeIndex = 0;
            }
            _localeName = Localization[_localeIndex];
        }
        
        public static string GetLanguageName()
        {
            return _localeName;
        }
        
        public static Language GetLanguage()
        {
            return _localeLang;
        }
    }
}
