using UnityEngine;

namespace EchoForge.Menu
{
    /// <summary>Acoustic reference wall - explains each component. Text authored in-scene.</summary>
    public sealed class SoundGuidePanel : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        public void Open() { if (panel != null) panel.SetActive(true); }
        public void Close() { if (panel != null) panel.SetActive(false); }
    }
}
