using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

namespace Prototype.AudioCore
{
    public class LanguageAudio : MonoBehaviour
    {
        private static Dictionary<string, AudioClip> _nonlanguageSounds;
        private static Dictionary<string, AudioClip> _languageSounds;

        private static readonly bool debug = false;

        private static string _multilanguagePath = "Sounds/MultiLanguage/";
        private static string _nonlanguagePath = "Sounds/NonLanguage/";
        
        public static void Init()
        {
            _nonlanguageSounds = new Dictionary<string, AudioClip>();
            _languageSounds = new Dictionary<string, AudioClip>();
            LoadSounds(false, false);
        }

        private static void ReleaseLanguageSounds()
        {
            _languageSounds.Clear();
        }
        
        public static AudioClip GetSoundByName(string name, bool multilanguage = true)
        {
            AudioClip sound = null;

            name = name.ToLower();

            if (_languageSounds != null && _languageSounds.ContainsKey(name))
            {
                sound = _languageSounds[name];
            }
            else if (_nonlanguageSounds != null && _nonlanguageSounds.ContainsKey(name))
            {
                sound = _nonlanguageSounds[name];
            }
            return sound;
        }
        
        private static string GetPath(string root, string name, string folder)
        {
            var path = root;
            if (folder != "")
            {
                path += folder + "/";
            }
            path += name;
            return path;
        }
        
        public static void LoadSounds(bool multilanguage = true, bool virt = true)
        {
            if (virt)
            {
                DOVirtual.DelayedCall(0, () => LoadSounds(multilanguage, false));
                return;
            }
            if (multilanguage)
            {
                ReleaseLanguageSounds();
                var lang = Languages.GetLanguageName();
                var path = _multilanguagePath + lang + "/";
                var res = Resources.LoadAll(path);
                for (var i = 0; i < res.Length; i++)
                {
                    var sound = res[i] as AudioClip;
                    StoreSound(_languageSounds, res[i].name, sound);
                    if (debug && sound == null)
                    {
                        print(path + "\t\t" + lang + "\t\t" + (sound));
                    }
                }
            }
            else
            {
                var res = Resources.LoadAll(_nonlanguagePath);
                for (var i = 0; i < res.Length; i++)
                {
                    var sound = res[i] as AudioClip;
                    StoreSound(_nonlanguageSounds, res[i].name, sound);
                    if (debug && sound == null)
                    {
                        print(_nonlanguagePath + "\t\t" + (sound));
                    }
                }
            }
        }
        
        public static void LoadSoundByName(string name, bool multilanguage = true, string folder = "")
        {
            if (multilanguage)
            {
                var lang = Languages.GetLanguageName();
                var path = GetPath(_multilanguagePath + lang + "/", name, folder);
                var sound = Resources.Load(path) as AudioClip;
                StoreSound(_languageSounds, name, sound);

                if (debug && sound == null)
                {
                    print(path + "\t\t" + lang + "\t\t" + (sound));
                }
            }
            else
            {
                var path = GetPath(_nonlanguagePath, name, folder);
                var sound = Resources.Load(path) as AudioClip;
                StoreSound(_nonlanguageSounds, name, sound);

                if (debug && sound == null)
                {
                    print(name + "\t\t" + (sound != null));
                }
            }
        }

      
        private static void StoreSound(Dictionary<string, AudioClip> dic, string name, AudioClip sound)
        {
            dic.Add(name.ToLower(), sound);
        }
    }
}
