using UnityEngine;
using TMPro;

namespace EchoForge.Chamber
{
    /// <summary>Scene-authored contextual chamber HUD. LevelController drives every method;
    /// authored buttons call back into LevelController. No layer is built at runtime.</summary>
    public sealed class ChamberUI : MonoBehaviour
    {
        [Header("Context readout (lower-left)")]
        [SerializeField] CanvasGroup contextGroup;
        [SerializeField] TMP_Text contextTitle;
        [SerializeField] TMP_Text contextDetail;

        [Header("Adjustment counter")]
        [SerializeField] TMP_Text tunesLabel;

        [Header("Tutorial layer")]
        [SerializeField] CanvasGroup tutorialGroup;
        [SerializeField] TMP_Text tutorialLabel;

        [Header("Result layer")]
        [SerializeField] GameObject resultLayer;
        [SerializeField] TMP_Text resultTitle;
        [SerializeField] TMP_Text resultReason;
        [SerializeField] TMP_Text resultRating;
        [SerializeField] GameObject nextButton;

        [Header("Pause layer")]
        [SerializeField] GameObject pauseLayer;

        [Header("Dialog layer")]
        [SerializeField] GameObject dialogLayer;
        [SerializeField] TMP_Text dialogLabel;

        [Header("Sound Guide layer")]
        [SerializeField] GameObject soundGuideLayer;

        void Awake()
        {
            HideContext();
            HideTutorial();
            HideResult();
            ShowPause(false);
            ShowDialog(null, false);
            ShowSoundGuide(false);
        }

        public void ShowContext(string title, string detail)
        {
            if (contextTitle) contextTitle.text = title;
            if (contextDetail) contextDetail.text = detail;
            SetGroup(contextGroup, true);
        }

        public void HideContext() => SetGroup(contextGroup, false);

        public void SetTunes(int n)
        {
            if (tunesLabel) tunesLabel.text = $"TUNES {Mathf.Clamp(n, 0, 99):00}";
        }

        public void ShowTutorial(string text)
        {
            if (tutorialLabel) tutorialLabel.text = text;
            SetGroup(tutorialGroup, !string.IsNullOrEmpty(text));
        }

        public void HideTutorial() => SetGroup(tutorialGroup, false);

        public void ShowResult(string title, string reason, string rating, bool showNext)
        {
            if (resultTitle) resultTitle.text = title;
            if (resultReason) resultReason.text = reason ?? "";
            if (resultRating) resultRating.text = rating ?? "";
            if (nextButton) nextButton.SetActive(showNext);
            if (resultLayer) resultLayer.SetActive(true);
        }

        public void HideResult()
        {
            if (resultLayer) resultLayer.SetActive(false);
        }

        public void ShowPause(bool on)
        {
            if (pauseLayer) pauseLayer.SetActive(on);
        }

        public void ShowDialog(string text, bool on)
        {
            if (dialogLabel && text != null) dialogLabel.text = text;
            if (dialogLayer) dialogLayer.SetActive(on);
        }

        public void ShowSoundGuide(bool on)
        {
            if (soundGuideLayer) soundGuideLayer.SetActive(on);
            if (pauseLayer && Time.timeScale == 0f) pauseLayer.SetActive(!on);
        }

        static void SetGroup(CanvasGroup g, bool on)
        {
            if (g == null) return;
            g.alpha = on ? 1f : 0f;
            g.blocksRaycasts = on;
            g.interactable = on;
        }
    }
}
