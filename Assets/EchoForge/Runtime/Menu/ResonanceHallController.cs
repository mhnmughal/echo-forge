using System.Collections;
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
        [SerializeField] Light rimLight;

        Vector3 _ringBaseScale = Vector3.one;
        float _rimBaseIntensity;

        void Start()
        {
            bool started = SaveSystem.Data.tutorialComplete;
            if (primaryActionLabel != null)
                primaryActionLabel.text = started ? "RESUME CHAMBER" : "START SESSION";

            if (resonanceRing != null) _ringBaseScale = resonanceRing.localScale;
            if (rimLight != null) _rimBaseIntensity = rimLight.intensity;

            AudioController.Instance?.PlayAmbience();

            bool inTune = SaveSystem.Data.chamberInTune;
            if (chamberInTuneEmblem != null) chamberInTuneEmblem.SetActive(inTune);

            if (inTune && !SaveSystem.Data.finaleSeen)
                StartCoroutine(Finale());
        }

        IEnumerator Finale()
        {
            // Chamber In Tune - the hall responds once. Later launches only show the emblem.
            AudioController.Instance?.Play(Sfx.FinalResonance);
            float t = 0f;
            while (t < 2.6f)
            {
                t += Time.deltaTime;
                float k = Mathf.Sin(Mathf.Clamp01(t / 2.6f) * Mathf.PI);
                if (resonanceRing != null)
                    resonanceRing.localScale = _ringBaseScale * (1f + 0.10f * k);
                if (rimLight != null)
                    rimLight.intensity = _rimBaseIntensity + 5f * k;
                yield return null;
            }
            if (resonanceRing != null) resonanceRing.localScale = _ringBaseScale;
            if (rimLight != null) rimLight.intensity = _rimBaseIntensity;

            SaveSystem.Data.finaleSeen = true;
            SaveSystem.Save();
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
            StopCoroutine(nameof(Ripple));
            StartCoroutine(nameof(Ripple));
        }

        IEnumerator Ripple()
        {
            if (resonanceRing == null) yield break;
            float t = 0f;
            while (t < 0.5f)
            {
                t += Time.deltaTime;
                float k = Mathf.Sin(Mathf.Clamp01(t / 0.5f) * Mathf.PI);
                resonanceRing.localScale = _ringBaseScale * (1f + 0.05f * k);
                yield return null;
            }
            resonanceRing.localScale = _ringBaseScale;
        }

        public void OnOpenDial() => dial?.Open(Mathf.Clamp(SaveSystem.Data.highestUnlocked, 1, 12));
        public void OnSoundCheck() => SceneFlow.LoadChamber(0);
        public void OnOpenSettings() => tuningStrip?.Open();
        public void OnOpenSoundGuide() => soundGuide?.Open();
    }
}
