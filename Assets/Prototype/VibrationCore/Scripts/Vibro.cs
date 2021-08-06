using MoreMountains.NiceVibrations;
using UnityEngine;


namespace Prototype.Vibration
{
    [System.Serializable]
    public enum TVibration
    {
        [Tooltip("Вже створені раніше")]
        Common,

        [Tooltip("Створюються самостійно")]
        Custom
    }

    public class Vibro : MonoBehaviour
    {
        public TVibration Type;

        public HapticTypes HapticTypes;

        public long[] Pattern;

        public int[] Amplitudes;

        public virtual void Trigger()
        {
            if (!GameSettings.IsVibroEnabled())
                return;

            if (Type == TVibration.Common)
            {
                MMVibrationManager.Haptic(HapticTypes);
            }
            else
            {
                if (MMVibrationManager.Android())
                {
                    MMVibrationManager.AndroidVibrate(Pattern, Amplitudes, -1);
                }
                else
                {
                    MMVibrationManager.iOSTriggerHaptics(HapticTypes, false);
                }
            }
        }
    }
}
