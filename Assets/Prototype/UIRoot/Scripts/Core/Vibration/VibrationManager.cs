using Core.Utility;
using UnityEngine;
using Prototype;
using MoreMountains.NiceVibrations;

namespace Core.Vibration
{
    public class VibrationManager : MonoBehaviourSingleton<VibrationManager>
    {
        public virtual void TriggerLightImpact()
        {
            if (!FeedbackStorage.IsVibroEnabled())
                return;

            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }

        public virtual void TriggerMediumImpact()
        {
            if (!FeedbackStorage.IsVibroEnabled())
                return;

            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        }

        public virtual void TriggerHeavyImpact()
        {
            if (!FeedbackStorage.IsVibroEnabled())
                return;

            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        }
    }
}