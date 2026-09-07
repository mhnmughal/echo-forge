using UnityEngine;

namespace EchoForge.Chamber
{
    /// <summary>Fits a RectTransform to the device safe area (notch / Dynamic Island / home
    /// indicator / rounded corners). Decorative chamber content lives outside this rect.</summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeAreaFitter : MonoBehaviour
    {
        RectTransform _rt;
        Rect _lastSafe;
        Vector2Int _lastScreen;

        void Awake() { _rt = GetComponent<RectTransform>(); Apply(); }

        void Update()
        {
            var screen = new Vector2Int(Screen.width, Screen.height);
            if (Screen.safeArea != _lastSafe || screen != _lastScreen) Apply();
        }

        void Apply()
        {
            _lastSafe = Screen.safeArea;
            _lastScreen = new Vector2Int(Screen.width, Screen.height);
            if (Screen.width <= 0 || Screen.height <= 0) return;

            Vector2 min = _lastSafe.position;
            Vector2 max = _lastSafe.position + _lastSafe.size;
            min.x /= Screen.width; min.y /= Screen.height;
            max.x /= Screen.width; max.y /= Screen.height;

            _rt.anchorMin = min;
            _rt.anchorMax = max;
            _rt.offsetMin = Vector2.zero;
            _rt.offsetMax = Vector2.zero;
        }
    }
}
