using UnityEngine;
using UnityEngine.UI;
using EchoForge.Game;
using EchoForge.Audio;

namespace EchoForge.Menu
{
    /// <summary>Settings as a minimal acoustic hardware strip: two faders, a haptics toggle,
    /// a recessed reset, a credits plate. Real systems, persisted.</summary>
    public sealed class TuningStripController : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] Slider musicFader;
        [SerializeField] Slider effectsFader;
        [SerializeField] Toggle hapticsToggle;
        [SerializeField] GameObject resetConfirm;
        [SerializeField] GameObject creditsPanel;

        public void Open()
        {
            if (panel != null) panel.SetActive(true);
            Pull();
        }

        public void Close()
        {
            if (panel != null) panel.SetActive(false);
            if (resetConfirm != null) resetConfirm.SetActive(false);
            if (creditsPanel != null) creditsPanel.SetActive(false);
        }

        void Pull()
        {
            if (musicFader != null) musicFader.SetValueWithoutNotify(SaveSystem.Data.musicVolume);
            if (effectsFader != null) effectsFader.SetValueWithoutNotify(SaveSystem.Data.effectsVolume);
            if (hapticsToggle != null) hapticsToggle.SetIsOnWithoutNotify(SaveSystem.Data.hapticsEnabled);
        }

        public void OnMusicChanged(float v)
        {
            SaveSystem.Data.musicVolume = Mathf.Clamp01(v);
            AudioController.Instance?.ApplyVolumes();
            SaveSystem.Save();
        }

        public void OnEffectsChanged(float v)
        {
            SaveSystem.Data.effectsVolume = Mathf.Clamp01(v);
            AudioController.Instance?.ApplyVolumes();
            AudioController.Instance?.Play(Sfx.SettingsTick);
            SaveSystem.Save();
        }

        public void OnHapticsChanged(bool on)
        {
            SaveSystem.Data.hapticsEnabled = on;
            SaveSystem.Save();
            if (on) Haptics.Play(Haptic.Light);
        }

        public void OnResetRequested() { if (resetConfirm != null) resetConfirm.SetActive(true); }
        public void OnResetCancelled() { if (resetConfirm != null) resetConfirm.SetActive(false); }

        public void OnResetConfirmed()
        {
            SaveSystem.ResetProgress();
            if (resetConfirm != null) resetConfirm.SetActive(false);
        }

        public void OnCreditsToggle()
        {
            if (creditsPanel != null) creditsPanel.SetActive(!creditsPanel.activeSelf);
        }
    }

    /// <summary>Acoustic reference wall - explains each component. Text authored in-scene.</summary>
    public sealed class SoundGuidePanel : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        public void Open() { if (panel != null) panel.SetActive(true); }
        public void Close() { if (panel != null) panel.SetActive(false); }
    }
}
