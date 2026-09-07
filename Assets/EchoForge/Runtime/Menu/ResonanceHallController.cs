using UnityEngine;
using TMPro;
using EchoForge.Game;
using EchoForge.Audio;

namespace EchoForge.Menu
{
    /// <summary>The circular acoustic main menu. Centerpiece is the Suspended Resonance Ring.
    /// No desk / cards / rack / console.</summary>
    public sealed class ResonanceHallController : MonoBehaviour
    {
        [SerializeField] ResonanceDialController dial;
        [SerializeField] TuningStripController tuningStrip;
        [SerializeField] SoundGuidePanel soundGuide;
        [SerializeField] TMP_Text primaryActionLabel;
        [SerializeField] GameObject chamberInTuneEmblem;
        [SerializeField] Transform resonanceRing;

        void Start()
        {
            bool started = SaveSystem.Data.tutorialComplete;
            if (primaryActionLabel != null)
                primaryActionLabel.text = started ? "RESUME CHAMBER" : "START SESSION";
            if (chamberInTuneEmblem != null)
                chamberInTuneEmblem.SetActive(SaveSystem.Data.chamberInTune);
            AudioController.Instance?.PlayAmbience();
        }

        public void OnPrimaryAction()
        {
            AudioController.Instance?.Play(Sfx.ResonanceDial);
            if (!SaveSystem.Data.tutorialComplete)
                SceneFlow.LoadChamber(0);
            else
                dial?.Open(Mathf.Clamp(SaveSystem.Data.highestUnlocked, 1, 12));
        }

        public void OnRingTapped()
        {
            AudioController.Instance?.Play(Sfx.ResonanceDial, Random.Range(0.97f, 1.03f));
            if (resonanceRing != null)
                resonanceRing.localScale = Vector3.one; // hook for a ripple tween later
        }

        public void OnOpenDial() => dial?.Open(Mathf.Clamp(SaveSystem.Data.highestUnlocked, 1, 12));
        public void OnSoundCheck() => SceneFlow.LoadChamber(0);
        public void OnOpenSettings() => tuningStrip?.Open();
        public void OnOpenSoundGuide() => soundGuide?.Open();
    }
}
