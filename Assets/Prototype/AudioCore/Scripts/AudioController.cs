using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;
using System.Linq;

namespace Prototype.AudioCore
{
    public static class AudioController
    {
        public static Action<bool> OnPause;
        
        public enum StreamGroup
        {
            FX,
            Voice,
            Music,
        }
        
        private static List<StreamGroup> _singleStream = new List<StreamGroup> {
        StreamGroup.Voice,
        StreamGroup.Music,
    };
        
        private static List<StreamGroup> _musicLikeStreams = new List<StreamGroup>()
    {
        StreamGroup.Music,
    };
        
        private static Dictionary<StreamGroup, float> _groupVolume = new Dictionary<StreamGroup, float>()
    {
        { StreamGroup.FX, 0.8f },
        { StreamGroup.Voice, 1f },
        { StreamGroup.Music, 0.8f },
    };
        
        private static Dictionary<StreamGroup, List<AudioSource>>
            _streams = new Dictionary<StreamGroup, List<AudioSource>>(),
            _userStreams = new Dictionary<StreamGroup, List<AudioSource>>(), 
            _pausedStreams = new Dictionary<StreamGroup, List<AudioSource>>(); 
        
        private const string
            PAUSE = "||";
        
        private static GameObject
            _srcParrent;
        
        private static Dictionary<StreamGroup, List<Sequence>>
            _tweens = new Dictionary<StreamGroup, List<Sequence>>(),
            _pausedTweens = new Dictionary<StreamGroup, List<Sequence>>();
        
        private static List<AudioSource>
            _lockedStreams = new List<AudioSource>();

       
        private static StreamGroup[]
            _availableGroups = null;

        public static StreamGroup[] AvailableGroups
        {
            get
            {
                if (_availableGroups == null)
                {
                    var A = System.Enum.GetValues(typeof(StreamGroup));
                    _availableGroups = new StreamGroup[A.Length];

                    for (var i = 0; i < A.Length; i++)
                    {
                        _availableGroups[i] = (StreamGroup)A.GetValue(i);
                    }
                }
                return _availableGroups;
            }
        }


        #region Methods to play sound by AudioClip.name (LanguageAudio is used to load sounds from Resources)
        public static void StopSound(string snd_name)
        {
            var container = GetStreams(snd_name);
            StopStreams(container);
        }
        
        public static void StopSound(string snd_name, StreamGroup group)
        {
            var container = GetStreams(snd_name, group);
            StopStreams(container);
        }
        
        public static bool IsSoundPlaying(string snd_name)
        {
            var lst = GetStreams(snd_name);
            return IsSoundPlaying(snd_name, lst);
        }
        
        public static bool IsSoundPlaying(string snd_name, StreamGroup group)
        {
            var lst = GetStreams(snd_name, group);
            return IsSoundPlaying(snd_name, lst);
        }
        
        private static bool IsSoundPlaying(string snd_name, List<AudioSource> lst)
        {
            var res = false;
            for (var i = 0; i < lst.Count; i++)
            {
                if (lst[i].IsPlaying())
                {
                    res = true;
                    break;
                }
            }
            return res;
        }
        
        public static float PlaySound(string snd_name, StreamGroup group = StreamGroup.FX, float volume = 1, bool loop = false, float pitch = 1)
        {
            AudioSource src = null;
            return PlaySound(snd_name, ref src, group, volume, loop, pitch);
        }
        
        public static float PlaySound(string snd_name, ref AudioSource _src, StreamGroup group = StreamGroup.FX, float volume = -1, bool loop = false, float pitch = 1)
        {
            float res = 0;
            if (snd_name != "")
            {
                var clip = LanguageAudio.GetSoundByName(snd_name);
                res = PlaySound(clip, ref _src, group, volume, loop, pitch);
            }
            else
            {
                Debug.Log("Play sound called with null argument");
            }

            return res;
        }
        
        public static float PlaySound(string[] snd_name, float interval = 0, StreamGroup group = StreamGroup.FX, float volume = 1)
        {
            Sequence stack = null;
            AudioSource _src = null;
            return PlaySound(snd_name, ref stack, ref _src, interval, group, volume);
        }
        
        public static float PlaySound(string[] snd_name, ref Sequence _stack, ref AudioSource _src, float interval = 0, StreamGroup group = StreamGroup.FX, float volume = 1)
        {
            var sounds = new AudioClip[snd_name.Length];
            var stack = DOTween.Sequence();
            _stack = stack;
            var container = GetTweenContainer(group);
            container.Add(stack);
            float t = 0;
            var src = GetStream(group);
            _src = src;
            LockStream(src, true);
            stack.OnComplete(() =>
          {
              container.Remove(stack);
              LockStream(src, false);
          });
            for (var i = 0; i < snd_name.Length; i++)
            {
                if (snd_name[i].Substring(0, 2).Equals(PAUSE))
                {
                    float delay;
                    if (float.TryParse(snd_name[i].Substring(2), out delay))
                    {
                        t += delay;
                    }
                }
                else
                {
                    sounds[i] = LanguageAudio.GetSoundByName(snd_name[i]);
                    var clip = sounds[i];
                    stack.InsertCallback(t, () =>
                  {
                      SetAndPlayStream(src, clip, group);
                  });
                    if (sounds[i] == null)
                    {
                        Debug.Log("Sound " + snd_name[i] + " is null");
                    }
                    else
                    {
                        t += sounds[i].length + interval;
                    }
                }
            }
            return Mathf.Clamp(t - interval, 0, float.MaxValue);
        }
        
        public static void CrossFadeMusic(string snd_name, float duration = 0.5f, AudioSource src = null)
        {
            var group = StreamGroup.Music;
            var fade_src = src == null ? GetStream(group) : src;
            var clip = LanguageAudio.GetSoundByName(snd_name);
            var fader = DOTween.Sequence();
            var container = GetTweenContainer(group);
            container.Add(fader);
            var vol = fade_src.volume;
            fader.Append(fade_src.DOFade(0, duration));
            fader.AppendCallback(() =>
          {
              fade_src.clip = clip;
              fade_src.Play();
          });
            fader.Append(fade_src.DOFade(vol, duration));
            fader.AppendCallback(() => container.Remove(fader));
        }
        
        public static float PlayMusic(string snd_name, float volume = -1)
        {
            return PlaySound(snd_name, StreamGroup.Music, volume, true);
        }
        
        public static float PlayVoice(string snd_name, float volume = -1, bool loop = false, float pitch = 1)
        {
            return PlaySound(snd_name, StreamGroup.Voice, volume, loop, pitch);
        }
        #endregion


        #region Methods to play sound as AudioClip
        public static float PlaySound(AudioClip clip, StreamGroup group = StreamGroup.FX, float volume = -1, bool loop = false, float pitch = 1)
        {
            AudioSource src = null;
            return PlaySound(clip, ref src, group, volume, loop, pitch);
        }
        
        public static float PlaySound(AudioClip clip, ref AudioSource _src, StreamGroup group = StreamGroup.FX, float volume = -1, bool loop = false, float pitch = 1)
        {
            float res = 0;

            if (clip != null)
            {
                _src = GetStream(group);
                res = SetAndPlayStream(_src, clip, group, volume, loop, pitch);
            }
            else
            {
                Debug.LogError("Не знайдено звука");
            }

            return res;
        }
        #endregion


        #region Tweens Management
        private static List<Sequence> GetTweenContainer(StreamGroup group, bool paused = false)
        {
            return GetContainer(group, paused ? _pausedTweens : _tweens);
        }
        
        public static void ReleaseTweens()
        {
            for (var i = 0; i < AvailableGroups.Length; i++)
            {
                ReleaseTweens(AvailableGroups[i]);
            }
        }
        
        public static void ReleaseTweens(StreamGroup group)
        {
            var container = GetTweenContainer(group);
            for (var i = 0; i < container.Count; i++)
            {
                container[i].Kill(true);
            }
            container.Clear();
        }
        
        public static void PauseTweens(bool pause)
        {
            for (var i = 0; i < AvailableGroups.Length; i++)
            {
                PauseTweens(pause, AvailableGroups[i]);
            }
        }
        
        public static void PauseTweens(bool pause, StreamGroup group)
        {
            var container = GetTweenContainer(group, true);

            if (pause)
            {
                container.AddRange(GetPlayingTweens(group).ToArray());
            }
            for (var i = 0; i < container.Count; i++)
            {
                if (pause)
                {
                    container[i].Pause();
                }
                else
                {
                    container[i].Play();
                }
            }
            if (!pause)
            {
                container.Clear();
            }
        }
        
        private static List<Sequence> GetPlayingTweens(StreamGroup group)
        {
            var res = new List<Sequence>();
            var container = GetTweenContainer(group);
            for (var i = 0; i < container.Count; i++)
            {
                if (container[i].IsPlaying())
                {
                    res.Add(container[i]);
                }
            }
            return res;
        }
        #endregion


        #region General methods
        public static void InitStreams(GameObject src_parrent = null)
        {
            _srcParrent = src_parrent;
            if (_srcParrent == null)
            {
                _srcParrent = new GameObject("AudioSources");
                GameObject.DontDestroyOnLoad(_srcParrent);
            }

            //добавити хоча б одне джерело для групи
            for (var i = 0; i < AvailableGroups.Length; i++)
            {
                GetStream(AvailableGroups[i]);
            }
        }
        
        private static List<T> GetContainer<T>(StreamGroup group, Dictionary<StreamGroup, List<T>> dic)
        {
            List<T> res = null;
            if (!dic.TryGetValue(group, out res))
            {
                res = new List<T>();
                dic.Add(group, res);
            }
            return res;
        }
        
        public static List<AudioSource> GetStreams(string snd_name)
        {
            var res = new List<AudioSource>();
            for (var i = 0; i < AvailableGroups.Length; i++)
            {
                res.AddRange(GetStreams(snd_name, AvailableGroups[i]));
            }
            return res;
        }
        
        public static List<AudioSource> GetStreams(string clip_name, StreamGroup group)
        {
            var res = new List<AudioSource>();
            res.AddRange(GetStreams(clip_name, GetStreamContainer((StreamGroup)group)));
            res.AddRange(GetStreams(clip_name, GetUserStreamContainer((StreamGroup)group))); 
            return res;
        }
        
        public static void Release(bool leave_music = false)
        {
            ReleaseTweens();
            
            for (var i = 0; i < AvailableGroups.Length; i++)
            {
                var group = AvailableGroups[i];
                if (!leave_music || group != StreamGroup.Music)
                {
                    ReleaseSources(group);
                }
            }
            
            _lockedStreams.Clear();
        }
        
        private static void EnableStreams(bool param, bool is_music)
        {
            for (var i = 0; i < AvailableGroups.Length; i++)
            {
                var group = AvailableGroups[i];
                if (_musicLikeStreams.Contains(group) == is_music)
                {
                    EnableStreams(GetStreamContainer(group), param);
                    EnableStreams(GetUserStreamContainer(group), param);
                }
            }
        }
        
        public static void EnableSounds(bool param)
        {
            EnableStreams(param, false);
        }
        
        public static void EnableMusic(bool param)
        {
            EnableStreams(param, true);
        }
        
        private static void SetStreamsVolume(float volume, bool is_music)
        {
            for (var i = 0; i < AvailableGroups.Length; i++)
            {
                var group = AvailableGroups[i];
                if (_musicLikeStreams.Contains(group) == is_music)
                {
                    SetStreamsVolume(volume, group);
                }
            }
        }
        
        public static void SetSoundsVolume(float volume)
        {
            SetStreamsVolume(volume, false);
        }
        
        public static void SetMusicVolume(float volume)
        {
            SetStreamsVolume(volume, true);
        }
        
        public static void Pause(bool pause)
        {
            PauseTweens(pause);

            for (var i = 0; i < AvailableGroups.Length; i++)
            {
                PauseStreams(pause, AvailableGroups[i]);
            }
            if (OnPause != null)
            {
                OnPause(pause);
            }
        }
        
        private static bool IsPlaying(this AudioSource src)
        {
            return src.isPlaying || src.time != 0;
        }
        #endregion


        #region StreamGroup management
        public static bool IsGroupEnabled(StreamGroup group)
        {
            var res = false;
            if (_musicLikeStreams.Contains(group))
            {
                res = FeedbackStorage.IsMusicEnabled();
            }
            else
            {
                res = FeedbackStorage.IsSoundsEnabled();
            }
            return res;
        }
        
        public static float GetStreamsVolume(StreamGroup group, float volume_override = -1)
        {
            float res = 0;
            if (_musicLikeStreams.Contains(group))
            {
                res = FeedbackStorage.GetMusicVol();
            }
            else
            {
                res = FeedbackStorage.GetSoundsVol();
            }

            if (volume_override >= 0 || _groupVolume.TryGetValue(group, out volume_override))
            {
                res *= volume_override;
            }
            return res;
        }
        
        private static void GetPlayingSources(StreamGroup group, ref List<AudioSource> receiver)
        {
            var src = new List<AudioSource>();
            GetPlayingStreams(GetStreamContainer(group), ref receiver);
            GetPlayingStreams(GetUserStreamContainer(group), ref receiver);
        }
        
        private static List<AudioSource> GetStreamContainer(StreamGroup group, bool paused = false)
        {
            return GetContainer(group, paused ? _pausedStreams : _streams);
        }
        
        public static float GetSoundsEndTime(StreamGroup group)
        {
            float res = 0;
            var container = GetStreamContainer(group);
            if (container != null && container.Count > 0)
            {
                res = container.FindAll(x => x.clip != null && x.IsPlaying() && !x.loop).Max(x => x.clip.length - x.time);
                Mathf.Clamp(res, 0, float.MaxValue);
            }
            return res;
        }
        
        public static void ReleaseSources(StreamGroup group)
        {
            ReleaseOwnSources(group);
            ReleaseUserStreams(group);
        }
        
        private static void ReleaseOwnSources(StreamGroup group)
        {
            var container = GetStreamContainer(group);
            for (var i = 0; i < container.Count; i++)
            {
                StopStream(container[i]);
            }
        }
        
        public static void SetGroupVolume(float volume, StreamGroup group)
        {
            if (!_groupVolume.ContainsKey(group))
            {
                _groupVolume.Add(group, volume);
            }
            else
            {
                _groupVolume[group] = volume;
            }
            SetStreamsVolume(volume, group);
        }
        
        public static void SetStreamsVolume(float volume, StreamGroup group)
        {
            volume = GetStreamsVolume(group, volume);
            SetVolume(GetStreamContainer(group), volume);
            SetVolume(GetUserStreamContainer(group), volume);
        }
        
        public static void PauseStreams(bool pause, StreamGroup group)
        {
            var container = GetStreamContainer(group, true);

            if (pause)
            {
                GetPlayingSources(group, ref container);
            }
            PauseStreams(container, pause);

            if (!pause)
            {
                container.Clear();
            }
        }
        #endregion


        #region AudioSource management
        private static AudioSource CreateStream()
        {
            if (_srcParrent == null)
            {
                InitStreams();
            }
            var src = _srcParrent.AddComponent<AudioSource>();
            src.playOnAwake = false;
            return src;
        }
        
        private static AudioSource GetStream(StreamGroup group)
        {
            AudioSource res = null;
            var container = GetStreamContainer(group);
            var single = _singleStream.Contains(group);
            for (var i = 0; container != null && i < container.Count; i++)
            {
                var src = container[i];

                if (!src.IsLocked() && (single || (!src.IsPlaying() && !src.IsPaused(group))))
                {
                    res = src;
                    break;
                }
            }
            if (res == null && (!single || container.Count == 0))
            {
                res = CreateStream();
                container.Add(res);
            }
            if (res == null)
            {
                Debug.LogError("Couldn't play sound on group " + group + " (is_single = " + single + "), but streams were locked or paused");
            }
            return res;
        }
        
        private static List<AudioSource> GetStreams(string snd_name, List<AudioSource> container)
        {
            return container.FindAll(X => X.clip != null && X.clip.name == snd_name);
        }
        
        private static float SetAndPlayStream(AudioSource src, AudioClip clip, StreamGroup group, float volume = -1, bool loop = false, float pitch = 1)
        {
            float res = 0;
            if (src != null)
            {
                if (clip != null)
                {
                    src.clip = clip;
                    res = clip.length;
                    src.loop = loop;
                    src.mute = !IsGroupEnabled(group);
                    src.volume = GetStreamsVolume(group, volume);
                    src.pitch = pitch;
                    src.Play();
                }
            }
            return res;
        }
        
        private static void GetPlayingStreams(List<AudioSource> container, ref List<AudioSource> receiver)
        {
            for (var i = 0; container != null && i < container.Count; i++)
            {
                if (container[i] != null)
                {
                    if (container[i].IsPlaying())
                    {
                        receiver.Add(container[i]);
                    }
                }
            }
        }
        
        public static void LockStream(AudioSource src, bool _lock)
        {
            if (_lock)
            {
                if (!_lockedStreams.Contains(src))
                {
                    _lockedStreams.Add(src);
                }
            }
            else
            {
                _lockedStreams.Remove(src);
            }
        }
        
        public static bool IsLocked(this AudioSource src)
        {
            return _lockedStreams.Contains(src);
        }
        
        private static bool IsPaused(this AudioSource src, StreamGroup group)
        {
            return _pausedStreams.ContainsKey(group) && _pausedStreams[group].Contains(src);
        }
        
        private static void PauseStreams(List<AudioSource> container, bool pause)
        {
            foreach (var source in container)
            {
                PauseStream(source, pause);
            }
            if (!pause)
            {
                container.Clear();
            }
        }
        
        private static void SetVolume(List<AudioSource> container, float volume)
        {
            for (var i = 0; container != null && i < container.Count; i++)
            {
                container[i].volume = volume;
            }
        }
        
        private static void EnableStreams(List<AudioSource> container, bool param)
        {
            foreach (var src in container)
            {
                if (src != null)
                {
                    src.mute = !param;
                }
            }
        }
        
        private static void PauseStream(AudioSource src, bool pause)
        {
            if (src != null)
            {
                if (pause)
                {
                    src.Pause();
                }
                else
                {
                    src.Play();
                }
            }
        }
        
        private static void StopStreams(List<AudioSource> container)
        {
            for (var i = 0; i < container.Count; i++)
            {
                StopStream(container[i]);
            }
        }
        
        private static void StopStream(AudioSource src)
        {
            if (src != null)
            {
                src.Stop();
                src.clip = null;
            }
        }
        #endregion


        #region User Sources management (deprecated)
        [Obsolete ( "Not used anymore, use method with ref AudioSource instead to get stream", false )]
        public static float PlayStream(AudioSource src, AudioClip clip = null, bool loop = false)
        {
            return PlaySound(clip, src, StreamGroup.FX, loop: loop);
        }

        [Obsolete ( "Not used anymore, use method with ref AudioSource instead to get stream", false )]
        public static float PlayMusic(AudioSource src, AudioClip clip = null, bool loop = true, float volume = -1)
        {
            return PlayMusic(clip, src, volume, loop);
        }

        [Obsolete ( "Not used anymore, use method with ref AudioSource instead to get stream", false )]
        private static float PlaySound(string snd_name, AudioSource src, StreamGroup group = StreamGroup.FX, float volume = -1, bool loop = false, float pitch = 1)
        {
            float res = 0;
            if (snd_name != "")
            {
                var clip = LanguageAudio.GetSoundByName(snd_name);
                res = PlaySound(clip, src, group, volume, loop, pitch);
            }
            else
            {
                Debug.Log("Play sound called with null argument");
            }
            return res;
        }

        [Obsolete ( "Not used anymore, use method with ref AudioSource instead to get stream", false )]
        public static float PlaySound(AudioClip clip, AudioSource src, StreamGroup group = StreamGroup.FX, float volume = -1, bool loop = false, float pitch = 1)
        {
            var res = SetAndPlayStream(src, clip, group, volume, loop, pitch);
            if (res > 0)
            {
                AddUserStream(src, group);
            }
            return res;
        }

        [Obsolete ( "Not used anymore, use method with ref AudioSource instead to get stream", false )]
        public static float PlayMusic(string snd_name, AudioSource src, float volume = -1)
        {
            return PlaySound(snd_name, src, StreamGroup.Music, volume, true);
        }

        [Obsolete ( "Not used anymore, use method with ref AudioSource instead to get stream", false )]
        public static float PlayMusic(AudioClip clip, AudioSource src, float volume = -1, bool loop = true)
        {
            return PlaySound(clip, src, StreamGroup.Music, volume, loop);
        }

        [Obsolete ( "Not used anymore", false )]
        private static List<AudioSource> GetUserStreamContainer(StreamGroup group)
        {
            return GetContainer(group, _userStreams);
        }

        [Obsolete ( "Not used anymore", false )]
        static private void AddUserStream(AudioSource src, StreamGroup group)
        {
            if (!_streams.ContainsKey(group))
            {
                _streams.Add(group, new List<AudioSource>());
            }
            var container = GetContainer(group, _userStreams);
            if (src != null && !container.Contains(src))
            {
                container.Add(src);
            }
        }

        [Obsolete ( "Not used anymore", false )]
        public static bool StopUserStream(AudioSource src, StreamGroup group)
        {
            StopStream(src);
            return RemoveUserStream(src, GetUserStreamContainer(group));
        }

        [Obsolete ( "Not used anymore", false )]
        private static void ReleaseUserStreams(StreamGroup group)
        {
            var container = GetUserStreamContainer(group);
            for (var i = 0; i < container.Count; i++)
            {
                StopStream(container[i]);
            }

            container.Clear();
        }

        [Obsolete ( "Not used anymore", false )]
        private static bool RemoveUserStream(AudioSource src, List<AudioSource> container)
        {
            var res = false;
            if (src != null && container.Contains(src))
            {
                res = container.Remove(src);
            }
            return res;
        }

        [Obsolete ( "Not used anymore", false )]
        public static bool StopUserStream(AudioSource src)
        {
            var res = false;
            StopStream(src);
            foreach (var item in _userStreams)
            {
                res = StopUserStream(src, item.Key);
                if (res)
                {
                    break;
                }
            }
            return res;
        }

        #endregion
    }
}
