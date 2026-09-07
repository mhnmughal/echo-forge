using UnityEngine;
using TMPro;

namespace EchoForge.Menu
{
    /// <summary>One chamber position on the Resonance Dial ring.</summary>
    public sealed class DialNode : MonoBehaviour
    {
        public int chamberId = 1;
        [SerializeField] TMP_Text numberLabel;
        [SerializeField] GameObject lockIcon;
        [SerializeField] GameObject focusGlow;
        [SerializeField] GameObject completeMark;

        public void Refresh(bool unlocked, bool completed, bool focused)
        {
            if (numberLabel != null) numberLabel.text = $"{chamberId:00}";
            if (lockIcon != null) lockIcon.SetActive(!unlocked);
            if (completeMark != null) completeMark.SetActive(completed);
            if (focusGlow != null) focusGlow.SetActive(focused);
        }
    }
}
