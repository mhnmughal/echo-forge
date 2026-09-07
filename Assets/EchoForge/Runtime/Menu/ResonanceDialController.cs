using UnityEngine;
using TMPro;
using EchoForge.Game;
using EchoForge.Audio;
using EchoForge.Level;

namespace EchoForge.Menu
{
    /// <summary>Radial chamber select. Twelve chambers sit at frequency positions on a ring the
    /// player rotates; the focused chamber reads out and launches. Not a grid / list / carousel.</summary>
    public sealed class ResonanceDialController : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] RectTransform ring;
        [SerializeField] DialNode[] nodes = new DialNode[12];
        [SerializeField] TMP_Text focusNumber;
        [SerializeField] TMP_Text focusTitle;
        [SerializeField] TMP_Text focusStatus;
        [SerializeField] UnityEngine.UI.Button launchButton;

        int _focus = 1;

        public void Open(int focus)
        {
            if (panel != null) panel.SetActive(true);
            _focus = Mathf.Clamp(focus, 1, 12);
            Layout();
        }

        public void Close() { if (panel != null) panel.SetActive(false); }

        public void Next() { Step(+1); }
        public void Prev() { Step(-1); }

        void Step(int d)
        {
            _focus = Mathf.Clamp(_focus + d, 1, 12);
            AudioController.Instance?.Play(Sfx.ResonanceDial, Random.Range(0.98f, 1.04f));
            Layout();
        }

        public void Launch()
        {
            if (SaveSystem.IsUnlocked(_focus))
                SceneFlow.LoadChamber(_focus);
        }

        public void LaunchSoundCheck() => SceneFlow.LoadChamber(0);

        void Layout()
        {
            if (ring != null)
                ring.localRotation = Quaternion.Euler(0f, 0f, (360f / 12f) * (_focus - 1));

            var catalog = LevelCatalog.Instance;
            for (int i = 0; i < nodes.Length; i++)
            {
                var n = nodes[i];
                if (n == null) continue;
                if (ring != null) n.transform.localRotation = Quaternion.Inverse(ring.localRotation);
                int id = n.chamberId;
                var rec = SaveSystem.RecordFor(id);
                n.Refresh(SaveSystem.IsUnlocked(id), rec.completed, id == _focus);
            }

            var def = catalog != null ? catalog.ById(_focus) : null;
            var frec = SaveSystem.RecordFor(_focus);
            if (focusNumber != null) focusNumber.text = $"{_focus:00}";
            if (focusTitle != null) focusTitle.text = def != null ? def.title : "";
            if (focusStatus != null)
                focusStatus.text = !SaveSystem.IsUnlocked(_focus) ? "LOCKED"
                    : frec.completed ? RatingWord(frec.bestRating) : "OPEN";
            if (launchButton != null) launchButton.interactable = SaveSystem.IsUnlocked(_focus);
        }

        static string RatingWord(int r) => r switch
        {
            3 => "PURE RESONANCE",
            2 => "CLEAN ECHO",
            1 => "RESOLVED",
            _ => "OPEN"
        };
    }
}
