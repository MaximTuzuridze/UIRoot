using System;
using Core.Utility;
using UnityEngine;

namespace Core.Sound
{
    public class SoundManager : MonoBehaviourSingleton<SoundManager>
    {
        public bool FXEnabled = true;
        public bool MusicEnabled = true;
        
        public void Play(AudioClip clip, Channel channel = Channel.FX)
        {
            switch (channel)
            {
                case Channel.FX:
                    if (!FXEnabled)
                        return;
                    break;
                case Channel.Music:
                    if (!MusicEnabled)
                        return;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(channel), channel, null);
            }
            Debug.Log("SoundManager : Play : " + clip);
        }

        public enum Channel
        {
            FX,
            Music
        }
    }
}