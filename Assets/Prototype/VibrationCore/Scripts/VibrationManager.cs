using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Prototype.Vibration
{
    [System.Serializable]
public class HapticTypeClass
{
    public string Name;

    public long[] Pattern;

    public int[] Amplitudes;

    public bool Repeat;
}

    public class VibrationManager : MonoBehaviour
    {
        public List<HapticTypeClass> HapticTypes;

        protected virtual void OnDisable()
        {
            MMVibrationManager.iOSReleaseHaptics();
        }

        protected virtual void Awake()
        {
            MMVibrationManager.iOSInitializeHaptics();
        }

        public virtual void TriggerNoDefault(string _name)
        {
            if (!GameSettings.IsVibroEnabled())
                return;

            if (MMVibrationManager.Android())
            {
                HapticTypeClass _hapticTypes = HapticTypes.Find((x) => x.Name == _name);
                if (_hapticTypes != null)
                {
                    MMVibrationManager.AndroidVibrate(_hapticTypes.Pattern, _hapticTypes.Amplitudes, _hapticTypes.Repeat ? 1 : -1);
                }
            }
            else
            {
                TriggerDefault();
            }
        }

        public virtual void TriggerNoDefault(int _id)
        {
            if (!GameSettings.IsVibroEnabled())
                return;

            if (MMVibrationManager.Android())
            {
                HapticTypeClass _hapticTypes = HapticTypes[_id];
                if (_hapticTypes != null)
                {
                    MMVibrationManager.AndroidVibrate(_hapticTypes.Pattern, _hapticTypes.Amplitudes, _hapticTypes.Repeat ? 1 : -1);
                }
            }
            else
            {
                TriggerDefault();
            }
        }

        public virtual void TriggerDefault()
        {
            if (!GameSettings.IsVibroEnabled())
                return;

#if UNITY_IOS || UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }

        public virtual void TriggerVibrate()
        {
            if (!GameSettings.IsVibroEnabled())
                return;

            MMVibrationManager.Vibrate();
        }

        public virtual void TriggerSelection()
        {
            if (!GameSettings.IsVibroEnabled())
                return;

            MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.Selection);
        }

        public virtual void TriggerSuccess()
        {
            if (!GameSettings.IsVibroEnabled())
                return;

            MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.Success);
        }

        public virtual void TriggerWarning()
        {
            if (!GameSettings.IsVibroEnabled())
                return;

            MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.Warning);
        }

        public virtual void TriggerFailure()
        {
            if (!GameSettings.IsVibroEnabled())
                return;

            MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.Failure);
        }

        public virtual void TriggerLightImpact()
        {
            if (!GameSettings.IsVibroEnabled())
                return;

            MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        }

        public virtual void TriggerMediumImpact()
        {
            if (!GameSettings.IsVibroEnabled())
                return;

            MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.MediumImpact);
        }

        public virtual void TriggerHeavyImpact()
        {
            if (!GameSettings.IsVibroEnabled())
                return;

            MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.HeavyImpact);
        }
    }
}
