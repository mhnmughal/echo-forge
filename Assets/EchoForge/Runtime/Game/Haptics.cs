using UnityEngine;

namespace EchoForge.Game
{
    public enum Haptic { Light, Medium, Heavy }

    /// <summary>Restrained device haptics, gated by the player setting. No-op in editor / on desktop.
    /// ponytail: base-Unity only has Handheld.Vibrate (coarse). Swap for a per-platform impact
    /// generator (iOS UIImpactFeedbackGenerator / Android VibrationEffect) if it feels blunt.</summary>
    public static class Haptics
    {
        public static void Play(Haptic strength)
        {
            if (!SaveSystem.Data.hapticsEnabled) return;
#if UNITY_IOS || UNITY_ANDROID
            if (Application.isMobilePlatform) Handheld.Vibrate();
#endif
        }
    }
}
